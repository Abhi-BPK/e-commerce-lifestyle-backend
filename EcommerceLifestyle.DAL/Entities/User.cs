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

    // Stores the BCrypt hash, NEVER the raw password.
    // Hashing is done in BLL/Services/BcryptPasswordHasher.cs.
    [Required, MaxLength(200)]
    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.User;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
