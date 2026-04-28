using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EcommerceLifestyle.DAL.Entities.Enums;

namespace EcommerceLifestyle.DAL.Entities;

// Order header row (one per placed order). Children live in OrderItem.
// Ship* columns flatten the shipping address — separate address tables are
// avoided because shipping addresses are immutable once an order is placed.
[Table("Orders")]
public class Order
{
    [Key, MaxLength(30)]
    public string Id { get; set; } = string.Empty; // e.g. ORD-20260428-123456

    public int UserId { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public OrderStatus Status { get; set; } = OrderStatus.Processing;

    [Column(TypeName = "decimal(10,2)")] public decimal Subtotal { get; set; }
    [Column(TypeName = "decimal(10,2)")] public decimal Shipping { get; set; }
    [Column(TypeName = "decimal(10,2)")] public decimal Tax { get; set; }
    [Column(TypeName = "decimal(10,2)")] public decimal Total { get; set; }

    [Required, MaxLength(80)]  public string ShipFirstName { get; set; } = string.Empty;
    [Required, MaxLength(80)]  public string ShipLastName  { get; set; } = string.Empty;
    [Required, MaxLength(200)] public string ShipLine1     { get; set; } = string.Empty;
    [Required, MaxLength(80)]  public string ShipCity      { get; set; } = string.Empty;
    [Required, MaxLength(80)]  public string ShipState     { get; set; } = string.Empty;
    [Required, MaxLength(20)]  public string ShipZip       { get; set; } = string.Empty;
    [Required, MaxLength(60)]  public string ShipCountry   { get; set; } = string.Empty;

    public List<OrderItem> Items { get; set; } = new();
}
