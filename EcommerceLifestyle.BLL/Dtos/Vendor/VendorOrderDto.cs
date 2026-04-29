namespace EcommerceLifestyle.BLL.Dtos.Vendor;

// Flattened order row for the vendor dashboard.
// One row per OrderItem belonging to a vendor's product.
// Status is PascalCase ("Pending" | "Processing" | "Shipped" | "Delivered" | "Cancelled")
// to match the FE tab labels exactly.
public class VendorOrderDto
{
    public string Id { get; set; } = string.Empty;          // OrderItem.Id (not OrderId)
    public string OrderId { get; set; } = string.Empty;     // Parent Order.Id
    public string BuyerName { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    public string Product { get; set; } = string.Empty;     // product display name
    public string? Size { get; set; }
    public string? Color { get; set; }
    public int Quantity { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime Date { get; set; }
    public string Address { get; set; } = string.Empty;     // single-line shipping address
}
