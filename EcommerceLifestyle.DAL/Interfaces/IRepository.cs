using System.Linq.Expressions;

namespace EcommerceLifestyle.DAL.Interfaces;

// Generic repository contract. Why generic?
//  - Re-use the same CRUD plumbing for every entity (DRY).
//  - Specific repos (IUserRepository, IProductRepository, ...) extend this
//    only where they need domain-tailored queries (e.g. GetByEmail).
public interface IRepository<TEntity, TKey> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
    Task AddAsync(TEntity entity, CancellationToken ct = default);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    IQueryable<TEntity> Query();
}
