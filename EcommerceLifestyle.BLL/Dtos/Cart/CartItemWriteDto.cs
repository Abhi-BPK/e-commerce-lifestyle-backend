using System.ComponentModel.DataAnnotations;

namespace EcommerceLifestyle.BLL.Dtos.Cart;

public class CartItemWriteDto
{
    [Required, MaxLength(20)]
    public string ProductId { get; set; } = string.Empty;

    [Range(1, 100)]
    public int Quantity { get; set; } = 1;
}
