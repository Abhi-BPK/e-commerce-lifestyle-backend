using System.Security.Claims;
using EcommerceLifestyle.BLL.Dtos.Users;
using EcommerceLifestyle.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceLifestyle.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _users;
    private readonly IConfiguration _config;

    public UsersController(IUserService users, IConfiguration config)
    {
        _users = users;
        _config = config;
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetMe(CancellationToken ct)
    {
        var dto = await _users.GetMeAsync(GetUserId(), ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPut("me")]
    public async Task<ActionResult<UserDto>> UpdateMe([FromBody] UserUpdateDto body, CancellationToken ct)
        => Ok(await _users.UpdateMeAsync(GetUserId(), body, ct));

    // Admin CRUD -- only enabled if Features:EnableUserAdminCrud=true.
    [Authorize(Roles = "Vendor")]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        if (!IsAdminCrudEnabled()) return NotFound();
        var users = await _users.GetAllAsync(ct);
        return Ok(users);
    }

    [Authorize(Roles = "Vendor")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetById(int id, CancellationToken ct)
    {
        if (!IsAdminCrudEnabled()) return NotFound();
        var user = await _users.GetByIdAsync(id, ct);
        return user is null ? NotFound() : Ok(user);
    }

    [Authorize(Roles = "Vendor")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        if (!IsAdminCrudEnabled()) return NotFound();
        await _users.DeleteAsync(id, ct);
        return NoContent();
    }

    private bool IsAdminCrudEnabled() =>
        _config.GetValue<bool>("Features:EnableUserAdminCrud");

    private int GetUserId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier)
               ?? User.FindFirstValue("sub")
               ?? "0";
        return int.Parse(raw);
    }
}
