using EcommerceLifestyle.DAL.Entities;
using EcommerceLifestyle.DAL.Interfaces;
using EcommerceLifestyle.DAL.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EcommerceLifestyle.DAL.Repositories;

public class ProductRepository : Repository<Product, string>, IProductRepository
{
    public ProductRepository(AppDbContext db) : base(db) { }

    public async Task<IEnumerable<Product>> GetBySubcategoryAsync(string slug, CancellationToken ct = default)
        => await _set.AsNoTracking()
                     .Where(p => p.Subcategory == slug)
                     .ToListAsync(ct);
}
