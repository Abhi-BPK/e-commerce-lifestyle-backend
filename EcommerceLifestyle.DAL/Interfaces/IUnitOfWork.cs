using Microsoft.EntityFrameworkCore.Storage;

namespace EcommerceLifestyle.DAL.Interfaces;

// Unit of Work pattern.
// Idea: many repositories share ONE DbContext instance.
// Services do reads/writes through repositories, then call SaveChangesAsync()
// once -- so all changes commit together (or roll back together).
public interface IUnitOfWork : IAsyncDisposable
{
    IUserRepository Users { get; }
    IProductRepository Products { get; }
    ICartRepository Cart { get; }
    IInventoryRepository Inventory { get; }
    IOrderRepository Orders { get; }
    ILogRepository Logs { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);
}
