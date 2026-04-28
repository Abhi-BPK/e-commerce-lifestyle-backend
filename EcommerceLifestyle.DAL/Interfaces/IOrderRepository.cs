using EcommerceLifestyle.DAL.Entities;

namespace EcommerceLifestyle.DAL.Interfaces;

public interface IOrderRepository : IRepository<Order, string>
{
    Task<IEnumerable<Order>> GetForUserAsync(int userId, CancellationToken ct = default);
    Task<Order?> GetWithItemsAsync(string orderId, CancellationToken ct = default);
}
