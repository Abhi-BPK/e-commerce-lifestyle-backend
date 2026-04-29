using EcommerceLifestyle.BLL.Dtos.Vendor;

namespace EcommerceLifestyle.BLL.Interfaces;

public interface IVendorOrderService
{
    Task<IEnumerable<VendorOrderDto>> GetForVendorAsync(int vendorId, CancellationToken ct = default);
    Task<VendorOrderDto> UpdateStatusAsync(int vendorId, int orderItemId, string newStatus, CancellationToken ct = default);
}
