using EcommerceLifestyle.BLL.Dtos.Cart;

namespace EcommerceLifestyle.BLL.Interfaces;

public interface ICartService
{
    Task<IEnumerable<CartItemDto>> GetForUserAsync(int userId, CancellationToken ct = default);
    Task<CartItemDto> UpsertAsync(int userId, CartItemWriteDto dto, CancellationToken ct = default);
    Task<CartItemDto?> UpdateQuantityAsync(int userId, int cartItemId, int quantity, CancellationToken ct = default);
    Task RemoveAsync(int userId, int cartItemId, CancellationToken ct = default);
    Task ClearAsync(int userId, CancellationToken ct = default);
}
