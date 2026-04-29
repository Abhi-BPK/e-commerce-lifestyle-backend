using System.Security.Claims;
using EcommerceLifestyle.BLL.Dtos.Vendor;
using EcommerceLifestyle.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceLifestyle.Api.Controllers.Vendor;

/// <summary>
/// Vendor-scoped per-variant inventory.
/// Returns ONE row per (Product, Size, Color) combination owned by the vendor.
/// </summary>
[ApiController]
[Authorize(Roles = "Vendor")]
[Route("api/vendor/inventory")]
public class VendorInventoryController : ControllerBase
{
    private readonly IVendorInventoryService _inventory;

    public VendorInventoryController(IVendorInventoryService inventory) => _inventory = inventory;

    /// <summary>List every variant owned by the calling vendor.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VendorInventoryDto>>> GetAll(CancellationToken ct)
        => Ok(await _inventory.GetForVendorAsync(GetVendorId(), ct));

    /// <summary>Update the stock quantity on a single variant the vendor owns.</summary>
    [HttpPatch("{id:int}")]
    public async Task<ActionResult<VendorInventoryDto>> UpdateStock(int id, [FromBody] VendorInventoryUpdateRequest body, CancellationToken ct)
        => Ok(await _inventory.UpdateStockAsync(GetVendorId(), id, body.Stock, ct));

    private int GetVendorId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier)
               ?? User.FindFirstValue("sub")
               ?? "0";
        return int.Parse(raw);
    }
}
