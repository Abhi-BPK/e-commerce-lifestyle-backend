namespace EcommerceLifestyle.BLL.Dtos.Cart;

// Cart row enriched with the joined product info -- saves the frontend an extra round-trip.
public class CartItemDto
{
    public int Id { get; set; }
    public string ProductId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Image { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime AddedAt { get; set; }
}
