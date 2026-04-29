using System.Security.Claims;
using EcommerceLifestyle.BLL.Dtos.Vendor;
using EcommerceLifestyle.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceLifestyle.Api.Controllers.Vendor;

/// <summary>
/// Vendor-scoped product management.
/// All endpoints require a JWT with Role="Vendor"; ownership of each product
/// is enforced inside the service layer.
/// </summary>
[ApiController]
[Authorize(Roles = "Vendor")]
[Route("api/vendor/products")]
public class VendorProductsController : ControllerBase
{
    private readonly IVendorProductService _products;

    public VendorProductsController(IVendorProductService products) => _products = products;

    /// <summary>List every product owned by the calling vendor.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VendorProductDto>>> GetAll(CancellationToken ct)
        => Ok(await _products.GetForVendorAsync(GetVendorId(), ct));

    /// <summary>Create a new product (and its size×color variants) for the calling vendor.</summary>
    [HttpPost]
    public async Task<ActionResult<VendorProductDto>> Create([FromBody] VendorProductWriteDto dto, CancellationToken ct)
    {
        var created = await _products.CreateAsync(GetVendorId(), dto, ct);
        return CreatedAtAction(nameof(GetAll), new { }, created);
    }

    /// <summary>Update an existing product owned by the calling vendor.</summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<VendorProductDto>> Update(string id, [FromBody] VendorProductWriteDto dto, CancellationToken ct)
        => Ok(await _products.UpdateAsync(GetVendorId(), id, dto, ct));

    /// <summary>Delete a product owned by the calling vendor (variants cascade).</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        await _products.DeleteAsync(GetVendorId(), id, ct);
        return NoContent();
    }

    /// <summary>Toggle a product's status between "active" and "inactive".</summary>
    [HttpPatch("{id}/status")]
    public async Task<ActionResult<VendorProductDto>> ToggleStatus(string id, CancellationToken ct)
        => Ok(await _products.ToggleStatusAsync(GetVendorId(), id, ct));

    // Same claim-extraction pattern as the rest of the controllers.
    private int GetVendorId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier)
               ?? User.FindFirstValue("sub")
               ?? "0";
        return int.Parse(raw);
    }
}
