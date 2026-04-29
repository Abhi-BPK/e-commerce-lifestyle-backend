using System.Security.Claims;
using EcommerceLifestyle.BLL.Dtos.Vendor;
using EcommerceLifestyle.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceLifestyle.Api.Controllers.Vendor;

/// <summary>
/// Vendor-scoped orders. Returns ONE row per OrderItem belonging to the
/// vendor's products (not one row per Order). Status is updated against
/// the parent Order under the hood.
/// </summary>
[ApiController]
[Authorize(Roles = "Vendor")]
[Route("api/vendor/orders")]
public class VendorOrdersController : ControllerBase
{
    private readonly IVendorOrderService _orders;

    public VendorOrdersController(IVendorOrderService orders) => _orders = orders;

    /// <summary>List every order line that contains one of the calling vendor's products.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VendorOrderDto>>> GetAll(CancellationToken ct)
        => Ok(await _orders.GetForVendorAsync(GetVendorId(), ct));

    /// <summary>
    /// Update the status of the order to which an order item belongs.
    /// {id} is the OrderItem id (matches the row id surfaced by GetAll).
    /// </summary>
    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<VendorOrderDto>> UpdateStatus(int id, [FromBody] VendorOrderStatusUpdateRequest body, CancellationToken ct)
        => Ok(await _orders.UpdateStatusAsync(GetVendorId(), id, body.Status, ct));

    private int GetVendorId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier)
               ?? User.FindFirstValue("sub")
               ?? "0";
        return int.Parse(raw);
    }
}
