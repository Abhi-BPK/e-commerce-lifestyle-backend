using System.Security.Claims;
using EcommerceLifestyle.BLL.Dtos.Cart;
using EcommerceLifestyle.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceLifestyle.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ICartService _cart;

    public CartController(ICartService cart) => _cart = cart;

    [HttpGet]
    public async Task<IActionResult> GetMine(CancellationToken ct)
    {
        var items = await _cart.GetForUserAsync(GetUserId(), ct);
        return Ok(new { items });
    }

    [HttpPost]
    public async Task<ActionResult<CartItemDto>> Upsert([FromBody] CartItemWriteDto dto, CancellationToken ct)
    {
        var item = await _cart.UpsertAsync(GetUserId(), dto, ct);
        return CreatedAtAction(nameof(GetMine), new { }, item);
    }

    public record UpdateQuantityRequest(int Quantity);

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CartItemDto>> UpdateQuantity(int id, [FromBody] UpdateQuantityRequest body, CancellationToken ct)
    {
        var updated = await _cart.UpdateQuantityAsync(GetUserId(), id, body.Quantity, ct);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remove(int id, CancellationToken ct)
    {
        await _cart.RemoveAsync(GetUserId(), id, ct);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Clear(CancellationToken ct)
    {
        await _cart.ClearAsync(GetUserId(), ct);
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
