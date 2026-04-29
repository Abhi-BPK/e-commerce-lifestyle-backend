using EcommerceLifestyle.BLL.Dtos.Vendor;
using EcommerceLifestyle.BLL.Interfaces;
using EcommerceLifestyle.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceLifestyle.BLL.Services;

// Aggregates the vendor dashboard's KPI cards + chart data.
//
// All aggregation is scoped to ORDER ITEMS whose Product.VendorId == vendorId,
// matching the row-per-item semantic the rest of the vendor dashboard uses.
public class VendorAnalyticsService : IVendorAnalyticsService
{
    private readonly IUnitOfWork _uow;

    public VendorAnalyticsService(IUnitOfWork uow) => _uow = uow;

    public async Task<VendorAnalyticsDto> GetForVendorAsync(int vendorId, CancellationToken ct = default)
    {
        // ── Pull every order line that belongs to this vendor's products ──
        var rows = await _uow.Orders.Query()
            .AsNoTracking()
            .SelectMany(o => o.Items, (o, i) => new { Order = o, Item = i })
            .Join(_uow.Products.Query(),
                  oi => oi.Item.ProductId,
                  p => p.Id,
                  (oi, p) => new { oi.Order, oi.Item, Product = p })
            .Where(x => x.Product.VendorId == vendorId)
            .Select(x => new
            {
                x.Order.Id,
                x.Order.Date,
                Status = x.Order.Status,
                ProductName = x.Item.Name,
                Quantity = x.Item.Quantity,
                LineTotal = x.Item.Price * x.Item.Quantity
            })
            .ToListAsync(ct);

        // ── Headline KPIs ──
        var totalRevenue = rows.Sum(r => r.LineTotal);
        var totalOrders  = rows.Count;
        var pendingOrders = rows.Count(r => r.Status == DAL.Entities.Enums.OrderStatus.Pending);

        // ── Low stock count ──
        // Defined the same way the FE inventory page defines it: stock < 5.
        var variants = await _uow.ProductVariants.GetForVendorAsync(vendorId, ct);
        var lowStockCount = variants.Count(v => v.Stock < 5);

        // ── Last 30 calendar days revenue ──
        // Build a [today-29 .. today] series so the chart always has 30 points
        // (gaps fill with 0). Date label format matches the FE mock ("Apr 1").
        var today = DateTime.UtcNow.Date;
        var revenueByDay = rows
            .Where(r => r.Date.Date >= today.AddDays(-29))
            .GroupBy(r => r.Date.Date)
            .ToDictionary(g => g.Key, g => g.Sum(r => r.LineTotal));

        var revenueData = Enumerable.Range(0, 30)
            .Select(offset => today.AddDays(-29 + offset))
            .Select(d => new VendorRevenuePoint
            {
                Date = d.ToString("MMM d"),
                Revenue = revenueByDay.TryGetValue(d, out var v) ? v : 0m
            })
            .ToList();

        // ── Top 5 products by units sold ──
        var topProducts = rows
            .GroupBy(r => r.ProductName)
            .Select(g => new VendorTopProduct { Name = g.Key, Units = g.Sum(r => r.Quantity) })
            .OrderByDescending(p => p.Units)
            .Take(5)
            .ToList();

        // ── Order status distribution ──
        // Always emit all 5 buckets so the FE pie chart slots are stable.
        var statusColors = new Dictionary<string, string>
        {
            ["Pending"]    = "#ff9500",
            ["Processing"] = "#007aff",
            ["Shipped"]    = "#5856d6",
            ["Delivered"]  = "#34c759",
            ["Cancelled"]  = "#ff3b30"
        };
        var orderStatusDist = statusColors.Select(kv => new VendorOrderStatusSlice
        {
            Name = kv.Key,
            Color = kv.Value,
            Value = rows.Count(r => r.Status.ToString() == kv.Key)
        }).ToList();

        return new VendorAnalyticsDto
        {
            TotalRevenue = totalRevenue,
            TotalOrders = totalOrders,
            PendingOrders = pendingOrders,
            LowStockCount = lowStockCount,
            RevenueData = revenueData,
            TopProducts = topProducts,
            OrderStatusDist = orderStatusDist
        };
    }
}
