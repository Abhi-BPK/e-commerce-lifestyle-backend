using EcommerceLifestyle.DAL.Entities;
using EcommerceLifestyle.DAL.Interfaces;
using EcommerceLifestyle.DAL.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EcommerceLifestyle.DAL.Repositories;

public class InventoryRepository : Repository<Inventory, int>, IInventoryRepository
{
    public InventoryRepository(AppDbContext db) : base(db) { }

    public Task<Inventory?> GetByProductIdAsync(string productId, CancellationToken ct = default)
        => _set.FirstOrDefaultAsync(i => i.ProductId == productId, ct);
}
