using EcommerceLifestyle.DAL.Entities;
using EcommerceLifestyle.DAL.Interfaces;
using EcommerceLifestyle.DAL.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EcommerceLifestyle.DAL.Repositories;

public class ProductVariantRepository : Repository<ProductVariant, int>, IProductVariantRepository
{
    public ProductVariantRepository(AppDbContext db) : base(db) { }

    public async Task<IEnumerable<ProductVariant>> GetForVendorAsync(int vendorId, CancellationToken ct = default)
        => await _set.AsNoTracking()
                     .Where(v => v.VendorId == vendorId)
                     .OrderBy(v => v.ProductName).ThenBy(v => v.Size).ThenBy(v => v.Color)
                     .ToListAsync(ct);

    public async Task<IEnumerable<ProductVariant>> GetForProductAsync(string productId, CancellationToken ct = default)
        => await _set.AsNoTracking()
                     .Where(v => v.ProductId == productId)
                     .ToListAsync(ct);
}
