using System.ComponentModel.DataAnnotations;

namespace EcommerceLifestyle.BLL.Dtos.Auth;

// Sent by the SPA after the OAuth provider redirects back with ?code=...
// The frontend just forwards the code + the redirect_uri it used (the
// provider requires the same redirect_uri at the token-exchange step).
public class OidcLoginRequest
{
    [Required]
    public string Code { get; set; } = string.Empty;

    [Required]
    public string RedirectUri { get; set; } = string.Empty;

    // Optional -- only present when the SPA used PKCE (recommended for Google).
    public string? CodeVerifier { get; set; }
}
