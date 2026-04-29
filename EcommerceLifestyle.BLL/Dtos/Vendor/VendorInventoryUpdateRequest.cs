using System.ComponentModel.DataAnnotations;

namespace EcommerceLifestyle.BLL.Dtos.Vendor;

// PATCH body for /api/vendor/inventory/{id}.
public class VendorInventoryUpdateRequest
{
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
}
