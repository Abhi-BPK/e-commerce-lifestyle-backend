using EcommerceLifestyle.BLL.Dtos.Vendor;

namespace EcommerceLifestyle.BLL.Interfaces;

public interface IVendorAnalyticsService
{
    Task<VendorAnalyticsDto> GetForVendorAsync(int vendorId, CancellationToken ct = default);
}
