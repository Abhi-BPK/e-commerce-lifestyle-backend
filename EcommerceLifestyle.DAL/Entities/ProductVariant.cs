using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceLifestyle.DAL.Entities;

// Per-variant stock for the vendor dashboard.
// Each row represents one (Product, Size, Color) combination owned by a vendor.
//
// This is a NEW table. The pre-existing `Inventory` table (one row per Product)
// is left untouched -- it still backs the public /api/inventory endpoints.
//
// Unique index on (ProductId, Size, Color) is configured in AppDbContext.
[Table("ProductVariants")]
public class ProductVariant
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(20)]
    public string ProductId { get; set; } = string.Empty;

    // Snapshot of the product name -- saves a join when listing inventory rows.
    [Required, MaxLength(160)]
    public string ProductName { get; set; } = string.Empty;

    [Required, MaxLength(8)]
    public string Size { get; set; } = string.Empty;   // e.g. "S", "M", "L"

    [Required, MaxLength(40)]
    public string Color { get; set; } = string.Empty;  // e.g. "White", "Navy"

    public int Stock { get; set; } = 0;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // The Vendor (User with Role=Vendor) that owns this variant.
    // Nullable to align with existing nullable VendorId pattern on Product.
    public int? VendorId { get; set; }
}
