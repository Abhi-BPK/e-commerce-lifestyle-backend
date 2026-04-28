using EcommerceLifestyle.BLL.Dtos.Auth;
using EcommerceLifestyle.BLL.Dtos.Cart;
using EcommerceLifestyle.BLL.Dtos.Inventory;
using EcommerceLifestyle.BLL.Dtos.Logs;
using EcommerceLifestyle.BLL.Dtos.Orders;
using EcommerceLifestyle.BLL.Dtos.Products;
using EcommerceLifestyle.BLL.Dtos.Users;
using EcommerceLifestyle.DAL.Entities;
using EcommerceLifestyle.DAL.Entities.Enums;

namespace EcommerceLifestyle.BLL.Mapping;

// Hand-written mappers (no AutoMapper). Trade-off:
//  - Pro: zero magic, easy to read, compile-time-checked.
//  - Con: each new field has to be added in two places (entity + mapper).
public static class DtoMappers
{
    // ---------- User ----------
    public static AuthUserDto ToAuthUserDto(User u) => new()
    {
        Id = u.Id,
        Email = u.Email,
        Role = u.Role.ToString(),
        FirstName = u.FirstName,
        LastName = u.LastName
    };

    public static UserDto ToUserDto(User u) => new()
    {
        Id = u.Id,
        FirstName = u.FirstName,
        LastName = u.LastName,
        Email = u.Email,
        Role = u.Role.ToString(),
        CreatedAt = u.CreatedAt
    };

    // ---------- Product ----------
    public static ProductDto ToDto(Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Slug = p.Slug,
        Subcategory = p.Subcategory,
        Price = p.Price,
        OriginalPrice = p.OriginalPrice,
        Rating = p.Rating,
        ReviewCount = p.ReviewCount,
        Badge = p.Badge,
        Description = p.Description,
        Image = p.Image,
        InStock = p.InStock,
        VendorId = p.VendorId
    };

    public static Product ToEntity(ProductWriteDto d, int? vendorId, string? idOverride = null) => new()
    {
        Id = idOverride ?? d.Id ?? Guid.NewGuid().ToString("N")[..12],
        Name = d.Name,
        Slug = d.Slug,
        Subcategory = d.Subcategory,
        Price = d.Price,
        OriginalPrice = d.OriginalPrice,
        Rating = d.Rating,
        ReviewCount = d.ReviewCount,
        Badge = d.Badge,
        Description = d.Description,
        Image = d.Image,
        InStock = d.InStock,
        VendorId = vendorId
    };

    public static void ApplyUpdate(Product entity, ProductWriteDto d)
    {
        entity.Name = d.Name;
        entity.Slug = d.Slug;
        entity.Subcategory = d.Subcategory;
        entity.Price = d.Price;
        entity.OriginalPrice = d.OriginalPrice;
        entity.Rating = d.Rating;
        entity.ReviewCount = d.ReviewCount;
        entity.Badge = d.Badge;
        entity.Description = d.Description;
        entity.Image = d.Image;
        entity.InStock = d.InStock;
    }

    // ---------- Cart ----------
    public static CartItemDto ToDto(Cart c, Product? p) => new()
    {
        Id = c.Id,
        ProductId = c.ProductId,
        Name = p?.Name ?? string.Empty,
        Price = p?.Price ?? 0m,
        Image = p?.Image ?? string.Empty,
        Quantity = c.Quantity,
        AddedAt = c.AddedAt
    };

    // ---------- Inventory ----------
    public static InventoryDto ToDto(Inventory i) => new()
    {
        Id = i.Id,
        ProductId = i.ProductId,
        QuantityAvailable = i.QuantityAvailable,
        ReorderLevel = i.ReorderLevel,
        LastUpdated = i.LastUpdated,
        VendorId = i.VendorId
    };

    // ---------- Order ----------
    public static OrderDto ToDto(Order o) => new()
    {
        Id = o.Id,
        UserId = o.UserId,
        Date = o.Date,
        Status = o.Status.ToString().ToLowerInvariant(), // "processing" etc
        Subtotal = o.Subtotal,
        Shipping = o.Shipping,
        Tax = o.Tax,
        Total = o.Total,
        Items = o.Items.Select(i => new OrderItemDto
        {
            ProductId = i.ProductId,
            Name = i.Name,
            Price = i.Price,
            Quantity = i.Quantity,
            Image = i.Image
        }).ToList(),
        ShippingAddress = new ShippingAddressDto
        {
            FirstName = o.ShipFirstName,
            LastName = o.ShipLastName,
            Line1 = o.ShipLine1,
            City = o.ShipCity,
            State = o.ShipState,
            Zip = o.ShipZip,
            Country = o.ShipCountry
        }
    };

    // ---------- Log ----------
    public static LogDto ToDto(Log l) => new()
    {
        Id = l.Id,
        Timestamp = l.Timestamp,
        Level = l.Level,
        Source = l.Source,
        Message = l.Message,
        UserId = l.UserId,
        RequestPath = l.RequestPath,
        Exception = l.Exception
    };
}
