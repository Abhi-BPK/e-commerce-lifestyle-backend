using EcommerceLifestyle.BLL.Dtos.Vendor;
using EcommerceLifestyle.BLL.Exceptions;
using EcommerceLifestyle.BLL.Interfaces;
using EcommerceLifestyle.DAL.Entities;
using EcommerceLifestyle.DAL.Entities.Enums;
using EcommerceLifestyle.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceLifestyle.BLL.Services;

// Vendor-scoped order view.
//
// The vendor dashboard shows ONE ROW PER OrderItem belonging to the vendor's
// products (not one row per Order). Status updates happen at the Order level
// because the underlying schema only stores Status on Order, not OrderItem --
// so updating one item's status updates its parent Order.
public class VendorOrderService : IVendorOrderService
{
    private readonly IUnitOfWork _uow;

    public VendorOrderService(IUnitOfWork uow) => _uow = uow;

    // ─────────────────────────── List ───────────────────────────
    public async Task<IEnumerable<VendorOrderDto>> GetForVendorAsync(int vendorId, CancellationToken ct = default)
    {
        // Single SQL query joining OrderItems -> Orders -> Products -> Users.
        // We use IQueryable.Query() rather than an in-memory loop so the join
        // ships down to MySQL and we don't blow up on large orders.
        var rows = await _uow.Orders.Query()
            .AsNoTracking()
            .SelectMany(o => o.Items, (o, i) => new { Order = o, Item = i })
            .Join(_uow.Products.Query(),
                  oi => oi.Item.ProductId,
                  p => p.Id,
                  (oi, p) => new { oi.Order, oi.Item, Product = p })
            .Where(x => x.Product.VendorId == vendorId)
            .Join(_uow.Users.Query(),
                  x => x.Order.UserId,
                  u => u.Id,
                  (x, u) => new { x.Order, x.Item, x.Product, Buyer = u })
            .OrderByDescending(x => x.Order.Date)
            .ToListAsync(ct);

        return rows.Select(x => new VendorOrderDto
        {
            Id = x.Item.Id.ToString(),
            OrderId = x.Order.Id,
            BuyerName = $"{x.Buyer.FirstName} {x.Buyer.LastName}".Trim(),
            ProductId = x.Item.ProductId,
            Product = x.Item.Name,
            Size = x.Item.Size,
            Color = x.Item.Color,
            Quantity = x.Item.Quantity,
            Total = x.Item.Price * x.Item.Quantity,
            Status = x.Order.Status.ToString(),  // PascalCase on the wire
            Date = x.Order.Date,
            Address = $"{x.Order.ShipLine1}, {x.Order.ShipCity}, {x.Order.ShipState} {x.Order.ShipZip}, {x.Order.ShipCountry}"
        });
    }

    // ─────────────────────── Update status ───────────────────────
    public async Task<VendorOrderDto> UpdateStatusAsync(int vendorId, int orderItemId, string newStatus, CancellationToken ct = default)
    {
        if (!Enum.TryParse<OrderStatus>(newStatus, ignoreCase: true, out var parsed))
        {
            throw new ApiException(400,
                "Status must be one of: Pending, Processing, Shipped, Delivered, Cancelled.",
                field: "status",
                code: "VALIDATION");
        }

        // Find the OrderItem + verify the vendor owns the underlying product.
        var row = await _uow.Orders.Query()
            .AsNoTracking()
            .SelectMany(o => o.Items, (o, i) => new { Order = o, Item = i })
            .Where(x => x.Item.Id == orderItemId)
            .Join(_uow.Products.Query(),
                  x => x.Item.ProductId,
                  p => p.Id,
                  (x, p) => new { x.Order, x.Item, Product = p })
            .Join(_uow.Users.Query(),
                  x => x.Order.UserId,
                  u => u.Id,
                  (x, u) => new { x.Order, x.Item, x.Product, Buyer = u })
            .FirstOrDefaultAsync(ct);

        if (row is null)
            throw new ApiException(404, "Order item not found.", code: "NOT_FOUND");

        if (row.Product.VendorId != vendorId)
            throw new ApiException(403, "You do not own the product on this order.", code: "FORBIDDEN");

        // Re-load the Order in tracked mode so we can update its status.
        var trackedOrder = await _uow.Orders.GetByIdAsync(row.Order.Id, ct)
            ?? throw new ApiException(404, "Order not found.", code: "NOT_FOUND");

        trackedOrder.Status = parsed;
        _uow.Orders.Update(trackedOrder);
        await _uow.SaveChangesAsync(ct);

        // Build the response from already-fetched values + new status.
        return new VendorOrderDto
        {
            Id = row.Item.Id.ToString(),
            OrderId = trackedOrder.Id,
            BuyerName = $"{row.Buyer.FirstName} {row.Buyer.LastName}".Trim(),
            ProductId = row.Item.ProductId,
            Product = row.Item.Name,
            Size = row.Item.Size,
            Color = row.Item.Color,
            Quantity = row.Item.Quantity,
            Total = row.Item.Price * row.Item.Quantity,
            Status = trackedOrder.Status.ToString(),
            Date = trackedOrder.Date,
            Address = $"{trackedOrder.ShipLine1}, {trackedOrder.ShipCity}, {trackedOrder.ShipState} {trackedOrder.ShipZip}, {trackedOrder.ShipCountry}"
        };
    }
}
