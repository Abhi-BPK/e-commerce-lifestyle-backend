using EcommerceLifestyle.BLL.Dtos.Vendor;

namespace EcommerceLifestyle.BLL.Interfaces;

public interface IVendorInventoryService
{
    Task<IEnumerable<VendorInventoryDto>> GetForVendorAsync(int vendorId, CancellationToken ct = default);
    Task<VendorInventoryDto> UpdateStockAsync(int vendorId, int variantId, int newStock, CancellationToken ct = default);
}
