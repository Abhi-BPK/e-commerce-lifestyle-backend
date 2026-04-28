using EcommerceLifestyle.DAL.Entities;

namespace EcommerceLifestyle.DAL.Interfaces;

public interface IInventoryRepository : IRepository<Inventory, int>
{
    Task<Inventory?> GetByProductIdAsync(string productId, CancellationToken ct = default);
}
