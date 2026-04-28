using System.ComponentModel.DataAnnotations;

namespace EcommerceLifestyle.BLL.Dtos.Products;

// Input DTO for vendor create/update of a product.
// Id is optional for create (the vendor may supply a slug-style id, e.g. "vp-001");
// for update, the route id wins.
public class ProductWriteDto
{
    [MaxLength(20)]
    public string? Id { get; set; }

    [Required, MaxLength(160)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(160)]
    public string Slug { get; set; } = string.Empty;

    [Required, MaxLength(40)]
    public string Subcategory { get; set; } = string.Empty;

    [Range(0, 1_000_000)]
    public decimal Price { get; set; }

    public decimal? OriginalPrice { get; set; }

    [Range(0, 5)]
    public decimal Rating { get; set; }

    [Range(0, int.MaxValue)]
    public int ReviewCount { get; set; }

    [MaxLength(20)]
    public string? Badge { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required, Url, MaxLength(500)]
    public string Image { get; set; } = string.Empty;

    public bool InStock { get; set; } = true;
}
