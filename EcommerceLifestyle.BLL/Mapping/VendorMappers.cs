using EcommerceLifestyle.BLL.Dtos.Vendor;
using EcommerceLifestyle.DAL.Entities;

namespace EcommerceLifestyle.BLL.Mapping;

// Vendor-specific entity <-> DTO mappers.
// Kept SEPARATE from the public DtoMappers.cs so we don't touch
// any existing mapper used by the public API.
public static class VendorMappers
{
    // ---- Variant -> InventoryDto ----
    public static VendorInventoryDto ToInventoryDto(ProductVariant v) => new()
    {
        Id = v.Id,
        ProductId = v.ProductId,
        ProductName = v.ProductName,
        Size = v.Size,
        Color = v.Color,
        Stock = v.Stock,
        Status = StockStatus(v.Stock),
        UpdatedAt = v.UpdatedAt
    };

    // FE rule (locked-in by the vendor inventory page):
    //   0      -> "Out of Stock"
    //   1..4   -> "Low Stock"
    //   >= 5   -> "In Stock"
    public static string StockStatus(int stock)
    {
        if (stock <= 0) return "Out of Stock";
        if (stock < 5) return "Low Stock";
        return "In Stock";
    }

    // ---- Product (+ its variants) -> VendorProductDto ----
    public static VendorProductDto ToProductDto(Product p, IReadOnlyList<ProductVariant> variants) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Category = p.Category ?? string.Empty,
        Gender = p.Gender ?? string.Empty,
        Price = p.Price,
        Stock = variants.Sum(v => v.Stock),
        Sizes = variants.Select(v => v.Size).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList(),
        Colors = variants.Select(v => v.Color).Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().ToList(),
        Status = string.IsNullOrWhiteSpace(p.Status) ? "active" : p.Status!,
        Image = p.Image,
        UnitsSold = p.UnitsSold
    };
}
