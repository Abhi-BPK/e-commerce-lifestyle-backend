using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceLifestyle.DAL.Entities;

// Product details table. The PK is a STRING (e.g. "pw-001") so backend ids
// match the ids the React frontend already uses for routing.
[Table("Products")]
public class Product
{
    [Key, MaxLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required, MaxLength(160)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(160)]
    public string Slug { get; set; } = string.Empty;

    // Subcategory slug -- one of: party-wear, ethnic-wear, formal,
    // casual, blazer-suits, funky. Indexed (see AppDbContext) because
    // GET /api/products/men?subcategory=... filters on it.
    [Required, MaxLength(40)]
    public string Subcategory { get; set; } = string.Empty;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? OriginalPrice { get; set; }

    [Column(TypeName = "decimal(2,1)")]
    public decimal Rating { get; set; }

    public int ReviewCount { get; set; }

    [MaxLength(20)]
    public string? Badge { get; set; }

    [Required, Column(TypeName = "TEXT")]
    public string Description { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string Image { get; set; } = string.Empty;

    public bool InStock { get; set; } = true;

    // Optional FK to the Vendor that owns the product.
    // Seeded products have null because they were imported from the frontend mock.
    public int? VendorId { get; set; }

    // ─── Vendor-dashboard additive fields (ALL nullable so existing seeded
    //     rows and existing public read paths continue to work unchanged) ───

    // Frontend taxonomy: "Men" | "Women" | "Unisex".
    // Independent of the existing Subcategory slug used by the public catalog.
    [MaxLength(20)]
    public string? Category { get; set; }

    // Frontend taxonomy: "Men" | "Women" | "Unisex".
    [MaxLength(20)]
    public string? Gender { get; set; }

    // Vendor-only soft-disable toggle: "active" | "inactive".
    // Null is treated as "active" for backwards compatibility.
    [MaxLength(16)]
    public string? Status { get; set; }

    // Lifetime units sold for this product (used by analytics top-products chart).
    // Defaults to 0; would be incremented when an OrderItem is placed for this product.
    public int UnitsSold { get; set; } = 0;
}
