using EcommerceLifestyle.BLL.Dtos.Vendor;

namespace EcommerceLifestyle.BLL.Interfaces;

// Vendor-scoped product CRUD. All methods enforce that the caller (vendorId)
// only sees / mutates products they own.
public interface IVendorProductService
{
    Task<IEnumerable<VendorProductDto>> GetForVendorAsync(int vendorId, CancellationToken ct = default);
    Task<VendorProductDto> CreateAsync(int vendorId, VendorProductWriteDto dto, CancellationToken ct = default);
    Task<VendorProductDto> UpdateAsync(int vendorId, string productId, VendorProductWriteDto dto, CancellationToken ct = default);
    Task DeleteAsync(int vendorId, string productId, CancellationToken ct = default);
    Task<VendorProductDto> ToggleStatusAsync(int vendorId, string productId, CancellationToken ct = default);
}
