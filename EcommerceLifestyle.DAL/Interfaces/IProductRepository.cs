using EcommerceLifestyle.DAL.Entities;

namespace EcommerceLifestyle.DAL.Interfaces;

public interface IProductRepository : IRepository<Product, string>
{
    Task<IEnumerable<Product>> GetBySubcategoryAsync(string slug, CancellationToken ct = default);
}
