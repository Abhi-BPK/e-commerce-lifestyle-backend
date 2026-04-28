using System.ComponentModel.DataAnnotations;

namespace EcommerceLifestyle.BLL.Dtos.Auth;

public class SignupRequest
{
    [Required, MaxLength(80)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(80)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(160)]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required, Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    // Optional -- if missing/blank, defaults to "User" in the service.
    public string? Role { get; set; }
}
