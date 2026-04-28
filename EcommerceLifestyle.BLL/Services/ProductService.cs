using EcommerceLifestyle.BLL.Dtos.Products;
using EcommerceLifestyle.BLL.Exceptions;
using EcommerceLifestyle.BLL.Interfaces;
using EcommerceLifestyle.BLL.Mapping;
using EcommerceLifestyle.DAL.Interfaces;

namespace EcommerceLifestyle.BLL.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _uow;

    public ProductService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<ProductDto>> GetMenAsync(string? subcategory, CancellationToken ct = default)
    {
        var products = string.IsNullOrWhiteSpace(subcategory)
            ? await _uow.Products.GetAllAsync(ct)
            : await _uow.Products.GetBySubcategoryAsync(subcategory, ct);

        return products.Select(DtoMappers.ToDto);
    }

    public async Task<ProductDto?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        var p = await _uow.Products.GetByIdAsync(id, ct);
        return p is null ? null : DtoMappers.ToDto(p);
    }

    public async Task<ProductDto> CreateAsync(ProductWriteDto dto, int vendorId, CancellationToken ct = default)
    {
        // Auto-generate id if not provided. Format roughly mimics seeded ids.
        var id = string.IsNullOrWhiteSpace(dto.Id)
            ? $"vp-{Guid.NewGuid().ToString("N")[..6]}"
            : dto.Id!;

        var existing = await _uow.Products.GetByIdAsync(id, ct);
        if (existing is not null)
            throw new ApiException(409, "A product with this id already exists.", field: "id", code: "DUPLICATE_ID");

        var entity = DtoMappers.ToEntity(dto, vendorId, id);
        await _uow.Products.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);
        return DtoMappers.ToDto(entity);
    }

    public async Task<ProductDto> UpdateAsync(string id, ProductWriteDto dto, CancellationToken ct = default)
    {
        var entity = await _uow.Products.GetByIdAsync(id, ct);
        if (entity is null)
            throw new ApiException(404, "Product not found.", code: "NOT_FOUND");

        DtoMappers.ApplyUpdate(entity, dto);
        _uow.Products.Update(entity);
        await _uow.SaveChangesAsync(ct);
        return DtoMappers.ToDto(entity);
    }

    public async Task DeleteAsync(string id, CancellationToken ct = default)
    {
        var entity = await _uow.Products.GetByIdAsync(id, ct);
        if (entity is null)
            throw new ApiException(404, "Product not found.", code: "NOT_FOUND");

        _uow.Products.Remove(entity);
        await _uow.SaveChangesAsync(ct);
    }
}
