namespace EcommerceLifestyle.BLL.Services;

// Provider-neutral shape returned by Google / GitHub clients.
// Keeps ExternalLoginService unaware of provider-specific JSON.
public sealed record ExternalUserInfo(
    string Provider,        // "Google" or "GitHub"
    string ProviderUserId,  // sub (Google) or numeric id (GitHub)
    string Email,
    string FirstName,
    string LastName);
