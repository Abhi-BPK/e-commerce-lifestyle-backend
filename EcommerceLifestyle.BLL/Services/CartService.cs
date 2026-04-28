using EcommerceLifestyle.BLL.Dtos.Cart;
using EcommerceLifestyle.BLL.Exceptions;
using EcommerceLifestyle.BLL.Interfaces;
using EcommerceLifestyle.BLL.Mapping;
using EcommerceLifestyle.DAL.Entities;
using EcommerceLifestyle.DAL.Interfaces;

namespace EcommerceLifestyle.BLL.Services;

public class CartService : ICartService
{
    private readonly IUnitOfWork _uow;

    public CartService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<CartItemDto>> GetForUserAsync(int userId, CancellationToken ct = default)
    {
        var rows = (await _uow.Cart.GetForUserAsync(userId, ct)).ToList();

        // Lookup all referenced products in one query (n+1 protection).
        var productIds = rows.Select(r => r.ProductId).Distinct().ToList();
        var products = (await _uow.Products.FindAsync(p => productIds.Contains(p.Id), ct))
                       .ToDictionary(p => p.Id);

        return rows.Select(r => DtoMappers.ToDto(r, products.GetValueOrDefault(r.ProductId)));
    }

    public async Task<CartItemDto> UpsertAsync(int userId, CartItemWriteDto dto, CancellationToken ct = default)
    {
        // Validate product exists.
        var product = await _uow.Products.GetByIdAsync(dto.ProductId, ct)
            ?? throw new ApiException(404, "Product not found.", field: "productId", code: "PRODUCT_NOT_FOUND");

        var existing = await _uow.Cart.GetByUserAndProductAsync(userId, dto.ProductId, ct);

        if (existing is null)
        {
            existing = new Cart
            {
                UserId = userId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                AddedAt = DateTime.UtcNow
            };
            await _uow.Cart.AddAsync(existing, ct);
        }
        else
        {
            // Same (UserId, ProductId) -- treat as quantity replace.
            existing.Quantity = dto.Quantity;
            _uow.Cart.Update(existing);
        }

        await _uow.SaveChangesAsync(ct);
        return DtoMappers.ToDto(existing, product);
    }

    public async Task<CartItemDto?> UpdateQuantityAsync(int userId, int cartItemId, int quantity, CancellationToken ct = default)
    {
        if (quantity < 1) throw new ApiException(400, "Quantity must be >= 1.", field: "quantity", code: "VALIDATION");

        var entity = await _uow.Cart.GetByIdAsync(cartItemId, ct);
        if (entity is null) return null;
        if (entity.UserId != userId) throw new ApiException(403, "Forbidden.", code: "FORBIDDEN");

        entity.Quantity = quantity;
        _uow.Cart.Update(entity);
        await _uow.SaveChangesAsync(ct);

        var product = await _uow.Products.GetByIdAsync(entity.ProductId, ct);
        return DtoMappers.ToDto(entity, product);
    }

    public async Task RemoveAsync(int userId, int cartItemId, CancellationToken ct = default)
    {
        var entity = await _uow.Cart.GetByIdAsync(cartItemId, ct);
        if (entity is null) return;
        if (entity.UserId != userId) throw new ApiException(403, "Forbidden.", code: "FORBIDDEN");

        _uow.Cart.Remove(entity);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task ClearAsync(int userId, CancellationToken ct = default)
    {
        var rows = await _uow.Cart.GetForUserAsync(userId, ct);
        foreach (var r in rows)
        {
            _uow.Cart.Remove(r);
        }
        await _uow.SaveChangesAsync(ct);
    }
}
