namespace EcommerceLifestyle.BLL.Dtos.Products;

// Output DTO for /api/products endpoints. Property names are camelCased
// during JSON serialisation (configured in Program.cs).
public class ProductDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Subcategory { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? OriginalPrice { get; set; }
    public decimal Rating { get; set; }
    public int ReviewCount { get; set; }
    public string? Badge { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public bool InStock { get; set; }
    public int? VendorId { get; set; }
}
