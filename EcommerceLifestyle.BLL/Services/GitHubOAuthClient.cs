using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using EcommerceLifestyle.BLL.Exceptions;
using Microsoft.Extensions.Configuration;

namespace EcommerceLifestyle.BLL.Services;

// GitHub uses plain OAuth 2.0 (no ID token). The flow:
//   1) POST code+client_id+client_secret to /login/oauth/access_token
//      -> { access_token, ... }
//   2) GET /user with the access_token  -> profile
//   3) GET /user/emails                 -> primary verified email
//
// The numeric "id" from /user is the stable provider user id.
//
// Config keys (appsettings.json):
//   Oidc:GitHub:ClientId
//   Oidc:GitHub:ClientSecret
public sealed class GitHubOAuthClient
{
    private const string TokenEndpoint = "https://github.com/login/oauth/access_token";
    private const string UserEndpoint = "https://api.github.com/user";
    private const string EmailsEndpoint = "https://api.github.com/user/emails";
    private const string UserAgent = "EcommerceLifestyle.Api";

    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public GitHubOAuthClient(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    public async Task<ExternalUserInfo> ExchangeCodeAsync(string code, string redirectUri, CancellationToken ct)
    {
        var section = _config.GetSection("Oidc:GitHub");
        var clientId = section["ClientId"];
        var clientSecret = section["ClientSecret"];
        if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
            throw new ApiException(500, "GitHub OAuth is not configured on the server.", code: "OIDC_NOT_CONFIGURED");

        // ── 1) Exchange code for access_token ──────────────────────────
        var form = new Dictionary<string, string>
        {
            ["client_id"] = clientId!,
            ["client_secret"] = clientSecret!,
            ["code"] = code,
            ["redirect_uri"] = redirectUri,
        };

        using var tokenReq = new HttpRequestMessage(HttpMethod.Post, TokenEndpoint)
        {
            Content = new FormUrlEncodedContent(form)
        };
        // GitHub returns text/plain by default; ask for JSON.
        tokenReq.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        tokenReq.Headers.UserAgent.ParseAdd(UserAgent);

        using var tokenResp = await _http.SendAsync(tokenReq, ct);
        if (!tokenResp.IsSuccessStatusCode)
        {
            var body = await tokenResp.Content.ReadAsStringAsync(ct);
            throw new ApiException(401, $"GitHub token exchange failed: {body}", code: "OIDC_EXCHANGE_FAILED");
        }

        var tokenJson = await tokenResp.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: ct);
        if (!tokenJson.TryGetProperty("access_token", out var accessTokenProp))
        {
            // GitHub returns 200 with { error, error_description } on failure.
            var err = tokenJson.TryGetProperty("error_description", out var d) ? d.GetString() : "no access_token";
            throw new ApiException(401, $"GitHub token exchange failed: {err}", code: "OIDC_EXCHANGE_FAILED");
        }
        var accessToken = accessTokenProp.GetString()!;

        // ── 2) Fetch user profile ──────────────────────────────────────
        var userJson = await GetJsonAsync(UserEndpoint, accessToken, ct);
        if (!userJson.TryGetProperty("id", out var idProp))
            throw new ApiException(401, "GitHub /user response is missing 'id'.", code: "OIDC_NO_SUB");

        // GitHub ids are integers; we store them as strings for portability.
        var providerUserId = idProp.ValueKind == JsonValueKind.Number
            ? idProp.GetInt64().ToString()
            : idProp.GetString() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(providerUserId))
            throw new ApiException(401, "GitHub /user 'id' was empty.", code: "OIDC_NO_SUB");

        var fullName = userJson.TryGetProperty("name", out var nameProp) ? nameProp.GetString() : null;
        var login = userJson.TryGetProperty("login", out var loginProp) ? loginProp.GetString() : null;
        var profileEmail = userJson.TryGetProperty("email", out var emailProp) && emailProp.ValueKind == JsonValueKind.String
            ? emailProp.GetString()
            : null;

        // ── 3) Fetch emails -- /user.email is null when the user hides it ──
        var email = profileEmail;
        if (string.IsNullOrWhiteSpace(email))
        {
            var emailsJson = await GetJsonAsync(EmailsEndpoint, accessToken, ct);
            if (emailsJson.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in emailsJson.EnumerateArray())
                {
                    var primary = item.TryGetProperty("primary", out var p) && p.ValueKind == JsonValueKind.True;
                    var verified = item.TryGetProperty("verified", out var v) && v.ValueKind == JsonValueKind.True;
                    if (primary && verified && item.TryGetProperty("email", out var e))
                    {
                        email = e.GetString();
                        break;
                    }
                }
            }
        }

        if (string.IsNullOrWhiteSpace(email))
            throw new ApiException(401, "Could not retrieve a verified primary email from GitHub. Ensure the 'user:email' scope is granted.", code: "OIDC_NO_EMAIL");

        var (first, last) = SplitName(fullName, login);
        return new ExternalUserInfo("GitHub", providerUserId, email!.Trim().ToLowerInvariant(), first, last);
    }

    private async Task<JsonElement> GetJsonAsync(string url, string accessToken, CancellationToken ct)
    {
        using var req = new HttpRequestMessage(HttpMethod.Get, url);
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
        req.Headers.UserAgent.ParseAdd(UserAgent);

        using var resp = await _http.SendAsync(req, ct);
        if (!resp.IsSuccessStatusCode)
        {
            var body = await resp.Content.ReadAsStringAsync(ct);
            throw new ApiException(401, $"GitHub GET {url} failed ({(int)resp.StatusCode}): {body}", code: "OIDC_USERINFO_FAILED");
        }

        return await resp.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: ct);
    }

    private static (string First, string Last) SplitName(string? full, string? login)
    {
        if (!string.IsNullOrWhiteSpace(full))
        {
            var parts = full!.Trim().Split(' ', 2);
            return parts.Length == 2 ? (parts[0], parts[1]) : (parts[0], string.Empty);
        }
        if (!string.IsNullOrWhiteSpace(login))
            return (login!, string.Empty);
        return ("GitHub", "User");
    }
}
