namespace EcommerceLifestyle.BLL.Dtos.Orders;

// Output DTO for /api/orders endpoints.
// Status is a lower-case string ("processing"/"shipped"/"delivered") to match the frontend.
public class OrderDto
{
    public string Id { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } = "processing";

    public decimal Subtotal { get; set; }
    public decimal Shipping { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }

    public List<OrderItemDto> Items { get; set; } = new();
    public ShippingAddressDto ShippingAddress { get; set; } = new();
}
