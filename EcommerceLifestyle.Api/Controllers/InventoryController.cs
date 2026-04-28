using System.Security.Claims;
using EcommerceLifestyle.BLL.Dtos.Inventory;
using EcommerceLifestyle.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceLifestyle.Api.Controllers;

[ApiController]
[Authorize(Roles = "Vendor")]
[Route("api/inventory")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventory;

    public InventoryController(IInventoryService inventory) => _inventory = inventory;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _inventory.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<InventoryDto>> GetById(int id, CancellationToken ct)
    {
        var row = await _inventory.GetByIdAsync(id, ct);
        return row is null ? NotFound() : Ok(row);
    }

    [HttpGet("by-product/{productId}")]
    public async Task<ActionResult<InventoryDto>> GetByProduct(string productId, CancellationToken ct)
    {
        var row = await _inventory.GetByProductIdAsync(productId, ct);
        return row is null ? NotFound() : Ok(row);
    }

    [HttpPost]
    public async Task<ActionResult<InventoryDto>> Create([FromBody] InventoryWriteDto dto, CancellationToken ct)
    {
        var created = await _inventory.CreateAsync(dto, GetUserId(), ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<InventoryDto>> Update(int id, [FromBody] InventoryWriteDto dto, CancellationToken ct)
        => Ok(await _inventory.UpdateAsync(id, dto, ct));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _inventory.DeleteAsync(id, ct);
        return NoContent();
    }

    private int GetUserId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier)
               ?? User.FindFirstValue("sub")
               ?? "0";
        return int.Parse(raw);
    }
}
