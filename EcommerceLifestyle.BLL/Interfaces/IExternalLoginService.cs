using EcommerceLifestyle.BLL.Dtos.Auth;

namespace EcommerceLifestyle.BLL.Interfaces;

public interface IExternalLoginService
{
    // Exchange an OAuth authorization code for our app's JWT.
    //   provider: "Google" or "GitHub" (case-insensitive)
    // Throws ApiException(400) for unknown providers,
    //        ApiException(401) when the code exchange or user lookup fails.
    Task<LoginResponse> LoginAsync(string provider, OidcLoginRequest req, CancellationToken ct = default);
}
