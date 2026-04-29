using EcommerceLifestyle.DAL.Entities;

namespace EcommerceLifestyle.DAL.Interfaces;

// Vendor-only repository.
// Same Repository<T> base class as the rest -- only the domain queries differ.
public interface IProductVariantRepository : IRepository<ProductVariant, int>
{
    /// <summary>Returns every variant a given vendor owns.</summary>
    Task<IEnumerable<ProductVariant>> GetForVendorAsync(int vendorId, CancellationToken ct = default);

    /// <summary>Returns every variant attached to a given product.</summary>
    Task<IEnumerable<ProductVariant>> GetForProductAsync(string productId, CancellationToken ct = default);
}
