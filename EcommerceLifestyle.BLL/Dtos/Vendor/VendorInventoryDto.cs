namespace EcommerceLifestyle.BLL.Dtos.Vendor;

// One row per (Product, Size, Color) variant.
// "Status" is computed server-side from Stock to match the FE rule
// (0 -> Out of Stock, 1-4 -> Low Stock, 5+ -> In Stock).
public class VendorInventoryDto
{
    public int Id { get; set; }
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int Stock { get; set; }
    public string Status { get; set; } = "In Stock";
    public DateTime UpdatedAt { get; set; }
}
