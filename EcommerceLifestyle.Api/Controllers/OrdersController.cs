using System.Security.Claims;
using EcommerceLifestyle.BLL.Dtos.Orders;
using EcommerceLifestyle.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceLifestyle.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orders;

    public OrdersController(IOrderService orders) => _orders = orders;

    [HttpGet]
    public async Task<IActionResult> GetMine(CancellationToken ct)
    {
        var orders = await _orders.GetForUserAsync(GetUserId(), ct);
        return Ok(new { orders });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken ct)
    {
        var order = await _orders.GetByIdForUserAsync(id, GetUserId(), ct);
        if (order is null) return NotFound();
        return Ok(new { order });
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> Place([FromBody] PlaceOrderRequest req, CancellationToken ct)
    {
        var created = await _orders.PlaceAsync(GetUserId(), req, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    private int GetUserId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier)
               ?? User.FindFirstValue("sub")
               ?? "0";
        return int.Parse(raw);
    }
}
