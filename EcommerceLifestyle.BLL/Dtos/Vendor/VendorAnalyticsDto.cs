namespace EcommerceLifestyle.BLL.Dtos.Vendor;

// Analytics aggregate for /api/vendor/analytics.
// Shape mirrors the React vendor dashboard's mock exactly.
public class VendorAnalyticsDto
{
    public decimal TotalRevenue { get; set; }
    public int TotalOrders { get; set; }
    public int PendingOrders { get; set; }
    public int LowStockCount { get; set; }

    public List<VendorRevenuePoint> RevenueData { get; set; } = new();
    public List<VendorTopProduct> TopProducts { get; set; } = new();
    public List<VendorOrderStatusSlice> OrderStatusDist { get; set; } = new();
}

public class VendorRevenuePoint
{
    public string Date { get; set; } = string.Empty;       // e.g. "Apr 1"
    public decimal Revenue { get; set; }
}

public class VendorTopProduct
{
    public string Name { get; set; } = string.Empty;
    public int Units { get; set; }
}

public class VendorOrderStatusSlice
{
    public string Name { get; set; } = string.Empty;       // status label
    public int Value { get; set; }                          // count
    public string Color { get; set; } = "#999999";         // hex color for the FE pie chart
}
