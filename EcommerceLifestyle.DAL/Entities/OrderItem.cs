using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceLifestyle.DAL.Entities;

// One line per product in an order. Name/Price/Image are SNAPSHOTTED
// at order time so historic orders don't change if the product is later edited.
[Table("OrderItems")]
public class OrderItem
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(30)]
    public string OrderId { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string ProductId { get; set; } = string.Empty;

    [Required, MaxLength(160)]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    public int Quantity { get; set; }

    [Required, MaxLength(500)]
    public string Image { get; set; } = string.Empty;
}
