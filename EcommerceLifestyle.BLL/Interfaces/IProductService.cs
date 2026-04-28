using EcommerceLifestyle.BLL.Dtos.Products;

namespace EcommerceLifestyle.BLL.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetMenAsync(string? subcategory, CancellationToken ct = default);
    Task<ProductDto?> GetByIdAsync(string id, CancellationToken ct = default);
    Task<ProductDto> CreateAsync(ProductWriteDto dto, int vendorId, CancellationToken ct = default);
    Task<ProductDto> UpdateAsync(string id, ProductWriteDto dto, CancellationToken ct = default);
    Task DeleteAsync(string id, CancellationToken ct = default);
}
