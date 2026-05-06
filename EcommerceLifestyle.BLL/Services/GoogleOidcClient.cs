using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Text.Json;
using EcommerceLifestyle.BLL.Exceptions;
using Microsoft.Extensions.Configuration;

namespace EcommerceLifestyle.BLL.Services;

// Talks to Google's OIDC endpoints. Pure HttpClient -- no Google.Apis.* package.
//
// Flow:
//   1) POST authorization code to https://oauth2.googleapis.com/token
//      -> Google returns { access_token, id_token, ... }
//   2) Decode the id_token (JWT). The "sub" claim is the stable user id.
//      We DO NOT verify the signature here -- we got the token directly from
//      Google over TLS using our client_secret, so the channel itself is the
//      proof of authenticity. (Same trade-off most server-side OIDC libraries
//      make for the "authorization code" flow.)
//
// Config keys (appsettings.json):
//   Oidc:Google:ClientId
//   Oidc:Google:ClientSecret
public sealed class GoogleOidcClient
{
    private const string TokenEndpoint = "https://oauth2.googleapis.com/token";

    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public GoogleOidcClient(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    public async Task<ExternalUserInfo> ExchangeCodeAsync(string code, string redirectUri, string? codeVerifier, CancellationToken ct)
    {
        var section = _config.GetSection("Oidc:Google");
        var clientId = section["ClientId"];
        var clientSecret = section["ClientSecret"];
        if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
            throw new ApiException(500, "Google OIDC is not configured on the server.", code: "OIDC_NOT_CONFIGURED");

        var form = new Dictionary<string, string>
        {
            ["code"] = code,
            ["client_id"] = clientId!,
            ["client_secret"] = clientSecret!,
            ["redirect_uri"] = redirectUri,
            ["grant_type"] = "authorization_code",
        };
        if (!string.IsNullOrWhiteSpace(codeVerifier))
            form["code_verifier"] = codeVerifier!;

        using var resp = await _http.PostAsync(TokenEndpoint, new FormUrlEncodedContent(form), ct);
        if (!resp.IsSuccessStatusCode)
        {
            var body = await resp.Content.ReadAsStringAsync(ct);
            throw new ApiException(401, $"Google token exchange failed: {body}", code: "OIDC_EXCHANGE_FAILED");
        }

        var tokenJson = await resp.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: ct);
        if (!tokenJson.TryGetProperty("id_token", out var idTokenProp))
            throw new ApiException(401, "Google did not return an id_token.", code: "OIDC_NO_ID_TOKEN");

        var idToken = idTokenProp.GetString();
        if (string.IsNullOrWhiteSpace(idToken))
            throw new ApiException(401, "Google id_token was empty.", code: "OIDC_NO_ID_TOKEN");

        // ── Decode id_token (JWS) ───────────────────────────────────────
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(idToken);
        var sub = jwt.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        var email = jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
        var given = jwt.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value;
        var family = jwt.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value;
        var name = jwt.Claims.FirstOrDefault(c => c.Type == "name")?.Value;

        if (string.IsNullOrWhiteSpace(sub))
            throw new ApiException(401, "Google id_token is missing the 'sub' claim.", code: "OIDC_NO_SUB");
        if (string.IsNullOrWhiteSpace(email))
            throw new ApiException(401, "Google id_token is missing 'email'. Make sure the 'email' scope was requested.", code: "OIDC_NO_EMAIL");

        var (first, last) = SplitName(given, family, name);
        return new ExternalUserInfo("Google", sub!, email!.Trim().ToLowerInvariant(), first, last);
    }

    private static (string First, string Last) SplitName(string? given, string? family, string? full)
    {
        if (!string.IsNullOrWhiteSpace(given) || !string.IsNullOrWhiteSpace(family))
            return (given ?? string.Empty, family ?? string.Empty);

        if (string.IsNullOrWhiteSpace(full))
            return ("Google", "User");

        var parts = full!.Trim().Split(' ', 2);
        return parts.Length == 2 ? (parts[0], parts[1]) : (parts[0], string.Empty);
    }
}
