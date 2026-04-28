using System.ComponentModel.DataAnnotations;

namespace EcommerceLifestyle.BLL.Dtos.Orders;

public class OrderItemDto
{
    [Required] public string ProductId { get; set; } = string.Empty;
    [Required] public string Name      { get; set; } = string.Empty;
    public decimal Price { get; set; }
    [Range(1, 100)] public int Quantity { get; set; }
    [Required] public string Image { get; set; } = string.Empty;
}
