using System.Linq.Expressions;
using EcommerceLifestyle.DAL.Interfaces;
using EcommerceLifestyle.DAL.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EcommerceLifestyle.DAL.Repositories;

// Generic CRUD base class. Concrete repositories inherit from this and add
// any domain-specific queries on top (e.g. UserRepository.GetByEmailAsync).
//
// Note: this class never calls SaveChangesAsync -- only the UnitOfWork does.
public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
{
    protected readonly AppDbContext _db;
    protected readonly DbSet<TEntity> _set;

    public Repository(AppDbContext db)
    {
        _db = db;
        _set = db.Set<TEntity>();
    }

    public virtual Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default)
        => _set.FindAsync(new object?[] { id! }, ct).AsTask();

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default)
        => await _set.AsNoTracking().ToListAsync(ct);

    public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        => await _set.AsNoTracking().Where(predicate).ToListAsync(ct);

    public virtual async Task AddAsync(TEntity entity, CancellationToken ct = default)
        => await _set.AddAsync(entity, ct);

    public virtual void Update(TEntity entity) => _set.Update(entity);

    public virtual void Remove(TEntity entity) => _set.Remove(entity);

    public IQueryable<TEntity> Query() => _set.AsQueryable();
}
