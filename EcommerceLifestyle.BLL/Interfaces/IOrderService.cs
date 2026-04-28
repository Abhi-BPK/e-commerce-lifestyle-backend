using EcommerceLifestyle.BLL.Dtos.Orders;

namespace EcommerceLifestyle.BLL.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetForUserAsync(int userId, CancellationToken ct = default);
    Task<OrderDto?> GetByIdForUserAsync(string orderId, int userId, CancellationToken ct = default);
    Task<OrderDto> PlaceAsync(int userId, PlaceOrderRequest req, CancellationToken ct = default);
}
