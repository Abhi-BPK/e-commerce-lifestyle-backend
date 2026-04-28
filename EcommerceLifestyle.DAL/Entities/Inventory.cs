using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceLifestyle.DAL.Entities;

// One inventory row per product (unique ProductId index in AppDbContext).
// Vendors maintain stock levels here.
[Table("Inventory")]
public class Inventory
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(20)]
    public string ProductId { get; set; } = string.Empty;

    public int QuantityAvailable { get; set; }

    public int ReorderLevel { get; set; } = 5;

    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    public int? VendorId { get; set; }
}
