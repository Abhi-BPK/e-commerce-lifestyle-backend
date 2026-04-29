using System.ComponentModel.DataAnnotations;

namespace EcommerceLifestyle.BLL.Dtos.Vendor;

// PATCH body for /api/vendor/orders/{id}/status.
// Accepted values (case-insensitive): Pending, Processing, Shipped, Delivered, Cancelled.
public class VendorOrderStatusUpdateRequest
{
    [Required, MaxLength(16)]
    public string Status { get; set; } = string.Empty;
}
