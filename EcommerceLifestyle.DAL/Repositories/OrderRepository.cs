using EcommerceLifestyle.DAL.Entities;
using EcommerceLifestyle.DAL.Interfaces;
using EcommerceLifestyle.DAL.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EcommerceLifestyle.DAL.Repositories;

public class OrderRepository : Repository<Order, string>, IOrderRepository
{
    public OrderRepository(AppDbContext db) : base(db) { }

    public async Task<IEnumerable<Order>> GetForUserAsync(int userId, CancellationToken ct = default)
        => await _set.AsNoTracking()
                     .Include(o => o.Items)
                     .Where(o => o.UserId == userId)
                     .OrderByDescending(o => o.Date)
                     .ToListAsync(ct);

    public Task<Order?> GetWithItemsAsync(string orderId, CancellationToken ct = default)
        => _set.AsNoTracking()
               .Include(o => o.Items)
               .FirstOrDefaultAsync(o => o.Id == orderId, ct);
}
