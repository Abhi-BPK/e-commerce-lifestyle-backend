using EcommerceLifestyle.DAL.Interfaces;
using EcommerceLifestyle.DAL.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace EcommerceLifestyle.DAL.Repositories;

// Concrete UnitOfWork. All repos share the SAME _db instance,
// so changes pile up in one ChangeTracker until SaveChangesAsync flushes them.
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;

    private IUserRepository? _users;
    private IProductRepository? _products;
    private ICartRepository? _cart;
    private IInventoryRepository? _inventory;
    private IOrderRepository? _orders;
    private ILogRepository? _logs;

    public UnitOfWork(AppDbContext db) => _db = db;

    public IUserRepository Users => _users ??= new UserRepository(_db);
    public IProductRepository Products => _products ??= new ProductRepository(_db);
    public ICartRepository Cart => _cart ??= new CartRepository(_db);
    public IInventoryRepository Inventory => _inventory ??= new InventoryRepository(_db);
    public IOrderRepository Orders => _orders ??= new OrderRepository(_db);
    public ILogRepository Logs => _logs ??= new LogRepository(_db);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
        => _db.Database.BeginTransactionAsync(ct);

    public async ValueTask DisposeAsync() => await _db.DisposeAsync();
}
