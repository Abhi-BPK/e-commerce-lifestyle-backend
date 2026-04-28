using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceLifestyle.DAL.Entities;

// One row per (User, Product). The unique index on (UserId, ProductId)
// is configured in AppDbContext so the same product cannot appear twice.
[Table("CartItems")]
public class Cart
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    [Required, MaxLength(20)]
    public string ProductId { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
