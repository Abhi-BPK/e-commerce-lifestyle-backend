using System.Security.Claims;
using EcommerceLifestyle.BLL.Dtos.Products;
using EcommerceLifestyle.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceLifestyle.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _products;

    public ProductsController(IProductService products) => _products = products;

    // GET /api/products/men?subcategory=party-wear -> { products: [...] }
    [AllowAnonymous]
    [HttpGet("men")]
    public async Task<IActionResult> GetMen([FromQuery] string? subcategory, CancellationToken ct)
    {
        var list = await _products.GetMenAsync(subcategory, ct);
        return Ok(new { products = list });
    }

    // GET /api/products/men/{id} -> { product }
    [AllowAnonymous]
    [HttpGet("men/{id}")]
    public async Task<IActionResult> GetMenById(string id, CancellationToken ct)
    {
        var p = await _products.GetByIdAsync(id, ct);
        if (p is null) return NotFound(new { status = 404, message = "Product not found.", code = "NOT_FOUND" });
        return Ok(new { product = p });
    }

    // GET /api/products -> generic CRUD list (anonymous read)
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? subcategory, CancellationToken ct)
    {
        var list = await _products.GetMenAsync(subcategory, ct);
        return Ok(list);
    }

    // GET /api/products/{id} -> single product (anonymous read)
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(string id, CancellationToken ct)
    {
        var p = await _products.GetByIdAsync(id, ct);
        return p is null ? NotFound() : Ok(p);
    }

    // POST /api/products -> Vendor only
    [Authorize(Roles = "Vendor")]
    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create([FromBody] ProductWriteDto dto, CancellationToken ct)
    {
        var vendorId = GetUserId();
        var created = await _products.CreateAsync(dto, vendorId, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT /api/products/{id} -> Vendor only
    [Authorize(Roles = "Vendor")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDto>> Update(string id, [FromBody] ProductWriteDto dto, CancellationToken ct)
        => Ok(await _products.UpdateAsync(id, dto, ct));

    // DELETE /api/products/{id} -> Vendor only
    [Authorize(Roles = "Vendor")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        await _products.DeleteAsync(id, ct);
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
