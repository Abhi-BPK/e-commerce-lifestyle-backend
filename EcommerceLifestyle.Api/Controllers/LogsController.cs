using EcommerceLifestyle.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceLifestyle.Api.Controllers;

[ApiController]
[Authorize(Roles = "Vendor")]
[Route("api/logs")]
public class LogsController : ControllerBase
{
    private readonly ILogService _logs;

    public LogsController(ILogService logs) => _logs = logs;

    // GET /api/logs?level=Error&since=2026-01-01&take=200
    [HttpGet]
    public async Task<IActionResult> Query(
        [FromQuery] string? level,
        [FromQuery] DateTime? since,
        [FromQuery] int take = 100,
        CancellationToken ct = default)
        => Ok(await _logs.QueryAsync(level, since, take, ct));

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var dto = await _logs.GetByIdAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }
}
