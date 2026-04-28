using EcommerceLifestyle.DAL.Entities;
using EcommerceLifestyle.DAL.Interfaces;
using EcommerceLifestyle.DAL.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EcommerceLifestyle.DAL.Repositories;

public class CartRepository : Repository<Cart, int>, ICartRepository
{
    public CartRepository(AppDbContext db) : base(db) { }

    public async Task<IEnumerable<Cart>> GetForUserAsync(int userId, CancellationToken ct = default)
        => await _set.AsNoTracking()
                     .Where(c => c.UserId == userId)
                     .ToListAsync(ct);

    public Task<Cart?> GetByUserAndProductAsync(int userId, string productId, CancellationToken ct = default)
        // Important: NOT AsNoTracking here. Caller may want to update the entity.
        => _set.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId, ct);
}
