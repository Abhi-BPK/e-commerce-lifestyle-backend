using EcommerceLifestyle.DAL.Entities;

namespace EcommerceLifestyle.DAL.Interfaces;

public interface ICartRepository : IRepository<Cart, int>
{
    Task<IEnumerable<Cart>> GetForUserAsync(int userId, CancellationToken ct = default);
    Task<Cart?> GetByUserAndProductAsync(int userId, string productId, CancellationToken ct = default);
}
