namespace EcommerceLifestyle.BLL.Dtos.Auth;

// Public-facing user shape returned by /auth/login.
// Mirrors the frontend authSlice user object exactly.
public class AuthUserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
