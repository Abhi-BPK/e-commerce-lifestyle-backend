using System.Security.Claims;
using EcommerceLifestyle.BLL.Dtos.Vendor;
using EcommerceLifestyle.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceLifestyle.Api.Controllers.Vendor;

/// <summary>
/// Vendor dashboard analytics. Returns the KPI cards (revenue, orders,
/// pending orders, low-stock count) plus the three chart series the
/// React Analytics page renders.
/// </summary>
[ApiController]
[Authorize(Roles = "Vendor")]
[Route("api/vendor/analytics")]
public class VendorAnalyticsController : ControllerBase
{
    private readonly IVendorAnalyticsService _analytics;

    public VendorAnalyticsController(IVendorAnalyticsService analytics) => _analytics = analytics;

    /// <summary>Get the calling vendor's dashboard analytics snapshot.</summary>
    [HttpGet]
    public async Task<ActionResult<VendorAnalyticsDto>> Get(CancellationToken ct)
        => Ok(await _analytics.GetForVendorAsync(GetVendorId(), ct));

    private int GetVendorId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier)
               ?? User.FindFirstValue("sub")
               ?? "0";
        return int.Parse(raw);
    }
}
