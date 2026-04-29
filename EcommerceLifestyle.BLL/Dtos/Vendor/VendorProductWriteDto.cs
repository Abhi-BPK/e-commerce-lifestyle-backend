using System.ComponentModel.DataAnnotations;

namespace EcommerceLifestyle.BLL.Dtos.Vendor;

// Body the React vendor "Add/Edit product" modal posts.
// Stock is the TOTAL across variants -- the backend distributes it
// across the (sizes × colors) cartesian product on create.
public class VendorProductWriteDto
{
    [Required, MaxLength(160)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Category { get; set; } = string.Empty;     // "Men" | "Women" | "Unisex"

    [Required, MaxLength(20)]
    public string Gender { get; set; } = string.Empty;       // "Men" | "Women" | "Unisex"

    [Range(0, 1_000_000)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    public List<string> Sizes { get; set; } = new();         // e.g. ["S","M","L"]
    public List<string> Colors { get; set; } = new();        // e.g. ["White","Blue"]

    [Required, Url, MaxLength(500)]
    public string Image { get; set; } = string.Empty;

    [MaxLength(16)]
    public string Status { get; set; } = "active";           // "active" | "inactive"
}
