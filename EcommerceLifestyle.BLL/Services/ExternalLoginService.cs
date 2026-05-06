using EcommerceLifestyle.BLL.Dtos.Auth;
using EcommerceLifestyle.BLL.Exceptions;
using EcommerceLifestyle.BLL.Interfaces;
using EcommerceLifestyle.BLL.Mapping;
using EcommerceLifestyle.DAL.Entities;
using EcommerceLifestyle.DAL.Entities.Enums;
using EcommerceLifestyle.DAL.Interfaces;

namespace EcommerceLifestyle.BLL.Services;

// Orchestrates external (Google / GitHub) login. Responsibilities:
//   1) Pick the provider client.
//   2) Find-or-create the local User by (Provider, ProviderUserId).
//   3) Hand back the SAME LoginResponse shape the password flow returns,
//      so the React frontend's authSlice can reuse its existing reducers.
//
// New users created here get Role = User (vendor accounts must be provisioned
// by the password signup flow as before -- this preserves existing privilege
// boundaries).
public class ExternalLoginService : IExternalLoginService
{
    private readonly IUnitOfWork _uow;
    private readonly ITokenService _tokens;
    private readonly GoogleOidcClient _google;
    private readonly GitHubOAuthClient _github;

    public ExternalLoginService(
        IUnitOfWork uow,
        ITokenService tokens,
        GoogleOidcClient google,
        GitHubOAuthClient github)
    {
        _uow = uow;
        _tokens = tokens;
        _google = google;
        _github = github;
    }

    public async Task<LoginResponse> LoginAsync(string provider, OidcLoginRequest req, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(req.Code))
            throw new ApiException(400, "Authorization code is required.", field: "code", code: "VALIDATION");
        if (string.IsNullOrWhiteSpace(req.RedirectUri))
            throw new ApiException(400, "Redirect URI is required.", field: "redirectUri", code: "VALIDATION");

        // Provider lookup is case-insensitive; we store the canonical casing on the row.
        ExternalUserInfo info = provider.Trim().ToLowerInvariant() switch
        {
            "google" => await _google.ExchangeCodeAsync(req.Code, req.RedirectUri, req.CodeVerifier, ct),
            "github" => await _github.ExchangeCodeAsync(req.Code, req.RedirectUri, ct),
            _ => throw new ApiException(400, $"Unsupported OIDC provider '{provider}'.", code: "UNSUPPORTED_PROVIDER"),
        };

        // ── 1) Already linked? ────────────────────────────────────────
        var user = await _uow.Users.GetByExternalLoginAsync(info.Provider, info.ProviderUserId, ct);

        if (user is null)
        {
            // ── 2) Email collision: an existing local account uses this email.
            // Refuse to silently take it over -- the legitimate owner could be
            // someone else. They must log in via password to link the account
            // (a future "link external account" feature can do this safely).
            var byEmail = await _uow.Users.GetByEmailAsync(info.Email, ct);
            if (byEmail is not null)
            {
                throw new ApiException(409,
                    $"An account with this email already exists. Sign in with your password and link {info.Provider} from your profile.",
                    field: "email",
                    code: "EMAIL_EXISTS_LOCAL");
            }

            // ── 3) Create a new user. PasswordHash stays NULL -- they sign in
            // exclusively via this provider until a password is set.
            user = new User
            {
                FirstName = string.IsNullOrWhiteSpace(info.FirstName) ? info.Provider : info.FirstName,
                LastName = string.IsNullOrWhiteSpace(info.LastName) ? "User" : info.LastName,
                Email = info.Email,
                PasswordHash = null,
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow,
                Provider = info.Provider,
                ProviderUserId = info.ProviderUserId,
            };

            await _uow.Users.AddAsync(user, ct);
            await _uow.SaveChangesAsync(ct);
        }

        return new LoginResponse
        {
            Token = _tokens.Create(user),
            User = DtoMappers.ToAuthUserDto(user),
        };
    }
}
