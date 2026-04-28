using System.ComponentModel.DataAnnotations;

namespace EcommerceLifestyle.BLL.Dtos.Inventory;

public class InventoryDto
{
    public int Id { get; set; }
    [Required, MaxLength(20)] public string ProductId { get; set; } = string.Empty;
    [Range(0, int.MaxValue)] public int QuantityAvailable { get; set; }
    [Range(0, int.MaxValue)] public int ReorderLevel { get; set; }
    public DateTime LastUpdated { get; set; }
    public int? VendorId { get; set; }
}
