using EcommerceLifestyle.BLL.Dtos.Orders;
using EcommerceLifestyle.BLL.Exceptions;
using EcommerceLifestyle.BLL.Interfaces;
using EcommerceLifestyle.BLL.Mapping;
using EcommerceLifestyle.DAL.Entities;
using EcommerceLifestyle.DAL.Entities.Enums;
using EcommerceLifestyle.DAL.Interfaces;

namespace EcommerceLifestyle.BLL.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _uow;

    public OrderService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<OrderDto>> GetForUserAsync(int userId, CancellationToken ct = default)
    {
        var orders = await _uow.Orders.GetForUserAsync(userId, ct);
        return orders.Select(DtoMappers.ToDto);
    }

    public async Task<OrderDto?> GetByIdForUserAsync(string orderId, int userId, CancellationToken ct = default)
    {
        var order = await _uow.Orders.GetWithItemsAsync(orderId, ct);
        if (order is null) return null;
        if (order.UserId != userId)
            throw new ApiException(403, "Forbidden.", code: "FORBIDDEN");
        return DtoMappers.ToDto(order);
    }

    public async Task<OrderDto> PlaceAsync(int userId, PlaceOrderRequest req, CancellationToken ct = default)
    {
        if (req.Items is null || req.Items.Count == 0)
            throw new ApiException(400, "Order must contain at least one item.", field: "items", code: "VALIDATION");

        // Build deterministic-ish id: ORD-yyyyMMdd-NNNNNN
        var id = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(100000, 999999)}";

        var order = new Order
        {
            Id = id,
            UserId = userId,
            Date = DateTime.UtcNow,
            Status = OrderStatus.Processing,
            Subtotal = req.Subtotal,
            Shipping = req.Shipping,
            Tax = req.Tax,
            Total = req.Total,
            ShipFirstName = req.ShippingAddress.FirstName,
            ShipLastName = req.ShippingAddress.LastName,
            ShipLine1 = req.ShippingAddress.Line1,
            ShipCity = req.ShippingAddress.City,
            ShipState = req.ShippingAddress.State,
            ShipZip = req.ShippingAddress.Zip,
            ShipCountry = req.ShippingAddress.Country,
            Items = req.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Name = i.Name,
                Price = i.Price,
                Quantity = i.Quantity,
                Image = i.Image
            }).ToList()
        };

        await _uow.Orders.AddAsync(order, ct);
        await _uow.SaveChangesAsync(ct);
        return DtoMappers.ToDto(order);
    }
}
