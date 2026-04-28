using System.ComponentModel.DataAnnotations;

namespace EcommerceLifestyle.BLL.Dtos.Orders;

// Body the frontend POSTs to /api/orders.
public class PlaceOrderRequest
{
    [Required, MinLength(1)]
    public List<OrderItemDto> Items { get; set; } = new();

    public decimal Subtotal { get; set; }
    public decimal Shipping { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }

    [Required]
    public ShippingAddressDto ShippingAddress { get; set; } = new();
}
