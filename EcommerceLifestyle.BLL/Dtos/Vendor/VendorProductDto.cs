namespace EcommerceLifestyle.BLL.Dtos.Vendor;

// Output DTO for /api/vendor/products endpoints.
// Shape mirrors the React vendor page (sizes/colors as flat arrays,
// "active"/"inactive" status, etc.) so no remapping is needed on the FE.
public class VendorProductDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;     // "Men" | "Women" | "Unisex"
    public string Gender { get; set; } = string.Empty;       // "Men" | "Women" | "Unisex"
    public decimal Price { get; set; }
    public int Stock { get; set; }                            // SUM of variant stocks
    public List<string> Sizes { get; set; } = new();          // distinct sizes from variants
    public List<string> Colors { get; set; } = new();         // distinct colors from variants
    public string Status { get; set; } = "active";           // "active" | "inactive"
    public string Image { get; set; } = string.Empty;
    public int UnitsSold { get; set; }
}
