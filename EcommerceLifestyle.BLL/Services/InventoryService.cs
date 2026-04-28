using EcommerceLifestyle.BLL.Dtos.Inventory;
using EcommerceLifestyle.BLL.Exceptions;
using EcommerceLifestyle.BLL.Interfaces;
using EcommerceLifestyle.BLL.Mapping;
using EcommerceLifestyle.DAL.Entities;
using EcommerceLifestyle.DAL.Interfaces;

namespace EcommerceLifestyle.BLL.Services;

public class InventoryService : IInventoryService
{
    private readonly IUnitOfWork _uow;

    public InventoryService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<InventoryDto>> GetAllAsync(CancellationToken ct = default)
    {
        var rows = await _uow.Inventory.GetAllAsync(ct);
        return rows.Select(DtoMappers.ToDto);
    }

    public async Task<InventoryDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var row = await _uow.Inventory.GetByIdAsync(id, ct);
        return row is null ? null : DtoMappers.ToDto(row);
    }

    public async Task<InventoryDto?> GetByProductIdAsync(string productId, CancellationToken ct = default)
    {
        var row = await _uow.Inventory.GetByProductIdAsync(productId, ct);
        return row is null ? null : DtoMappers.ToDto(row);
    }

    public async Task<InventoryDto> CreateAsync(InventoryWriteDto dto, int? vendorId, CancellationToken ct = default)
    {
        var product = await _uow.Products.GetByIdAsync(dto.ProductId, ct)
            ?? throw new ApiException(404, "Product not found.", field: "productId", code: "PRODUCT_NOT_FOUND");

        var existing = await _uow.Inventory.GetByProductIdAsync(dto.ProductId, ct);
        if (existing is not null)
            throw new ApiException(409, "Inventory row already exists for this product.", field: "productId", code: "DUPLICATE");

        var entity = new Inventory
        {
            ProductId = dto.ProductId,
            QuantityAvailable = dto.QuantityAvailable,
            ReorderLevel = dto.ReorderLevel,
            LastUpdated = DateTime.UtcNow,
            VendorId = vendorId
        };
        await _uow.Inventory.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);
        return DtoMappers.ToDto(entity);
    }

    public async Task<InventoryDto> UpdateAsync(int id, InventoryWriteDto dto, CancellationToken ct = default)
    {
        var entity = await _uow.Inventory.GetByIdAsync(id, ct);
        if (entity is null)
            throw new ApiException(404, "Inventory row not found.", code: "NOT_FOUND");

        entity.ProductId = dto.ProductId;
        entity.QuantityAvailable = dto.QuantityAvailable;
        entity.ReorderLevel = dto.ReorderLevel;
        entity.LastUpdated = DateTime.UtcNow;
        _uow.Inventory.Update(entity);
        await _uow.SaveChangesAsync(ct);
        return DtoMappers.ToDto(entity);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _uow.Inventory.GetByIdAsync(id, ct);
        if (entity is null)
            throw new ApiException(404, "Inventory row not found.", code: "NOT_FOUND");

        _uow.Inventory.Remove(entity);
        await _uow.SaveChangesAsync(ct);
    }
}
