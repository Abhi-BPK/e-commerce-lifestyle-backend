using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EcommerceLifestyle.DAL.Entities.Enums;

namespace EcommerceLifestyle.DAL.Entities;

// User details table. Holds both User-role customers and Vendor-role sellers.
[Table("Users")]
public class User
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(80)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(80)]
    public string LastName { get; set; } = string.Empty;

    [Required, MaxLength(160)]
    public string Email { get; set; } = string.Empty;

    // Stores the BCrypt hash for local users. NULL for users created via
    // external OIDC providers (Google / GitHub) -- they authenticate through
    // the provider, never with a local password.
    // Hashing is done in BLL/Services/BcryptPasswordHasher.cs.
    [MaxLength(200)]
    public string? PasswordHash { get; set; }

    public UserRole Role { get; set; } = UserRole.User;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // ---------- External login (OIDC / OAuth) ----------
    // "Local" for password users, "Google" or "GitHub" for external accounts.
    // Existing rows are backfilled to "Local" by the migration.
    [MaxLength(20)]
    public string Provider { get; set; } = "Local";

    // The stable user identifier issued by the external provider.
    //   Google -> the "sub" claim of the ID token
    //   GitHub -> the numeric user id from /user
    // Null for local users. Combined with Provider, must be unique.
    [MaxLength(100)]
    public string? ProviderUserId { get; set; }
}
