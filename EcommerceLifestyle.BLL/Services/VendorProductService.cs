using EcommerceLifestyle.BLL.Dtos.Vendor;
using EcommerceLifestyle.BLL.Exceptions;
using EcommerceLifestyle.BLL.Interfaces;
using EcommerceLifestyle.BLL.Mapping;
using EcommerceLifestyle.DAL.Entities;
using EcommerceLifestyle.DAL.Interfaces;

namespace EcommerceLifestyle.BLL.Services;

// Vendor-scoped product CRUD.
//
// Notes:
//  - Every read query is filtered by Product.VendorId == vendorId.
//  - Create/Update/Delete also assert ownership; mismatched vendorId -> 403.
//  - On create, the supplied total Stock is distributed evenly across the
//    cartesian product of (Sizes x Colors) variants.
public class VendorProductService : IVendorProductService
{
    private readonly IUnitOfWork _uow;

    public VendorProductService(IUnitOfWork uow) => _uow = uow;

    // ───────────────────────────── List ─────────────────────────────
    public async Task<IEnumerable<VendorProductDto>> GetForVendorAsync(int vendorId, CancellationToken ct = default)
    {
        var products = await _uow.Products.FindAsync(p => p.VendorId == vendorId, ct);
        var productList = products.ToList();
        if (productList.Count == 0) return Enumerable.Empty<VendorProductDto>();

        // Bulk-load all variants for all of this vendor's products in one query.
        var allVariants = (await _uow.ProductVariants.GetForVendorAsync(vendorId, ct)).ToList();

        return productList.Select(p =>
        {
            var variants = allVariants.Where(v => v.ProductId == p.Id).ToList();
            return VendorMappers.ToProductDto(p, variants);
        });
    }

    // ───────────────────────────── Create ─────────────────────────────
    public async Task<VendorProductDto> CreateAsync(int vendorId, VendorProductWriteDto dto, CancellationToken ct = default)
    {
        // Generate ID + slug from name (vendor-namespaced).
        var id = $"vp-{Guid.NewGuid().ToString("N")[..8]}";
        var slug = MakeSlug(dto.Name) + "-" + id;

        var product = new Product
        {
            Id = id,
            Name = dto.Name.Trim(),
            Slug = slug,
            // Map vendor "category" to the legacy Subcategory column so
            // the existing public listing (which filters on Subcategory)
            // still works without code changes.
            Subcategory = (dto.Category ?? string.Empty).ToLowerInvariant(),
            Price = dto.Price,
            Rating = 0m,
            ReviewCount = 0,
            Description = dto.Name,    // sensible default; FE doesn't yet collect this
            Image = dto.Image,
            InStock = dto.Stock > 0,
            VendorId = vendorId,
            Category = dto.Category,
            Gender = dto.Gender,
            Status = NormalizeStatus(dto.Status),
            UnitsSold = 0
        };

        await _uow.Products.AddAsync(product, ct);

        // Build variants (size × color cartesian); distribute stock evenly.
        var variants = BuildVariants(product, dto, vendorId);
        foreach (var v in variants)
        {
            await _uow.ProductVariants.AddAsync(v, ct);
        }

        await _uow.SaveChangesAsync(ct);

        return VendorMappers.ToProductDto(product, variants);
    }

    // ───────────────────────────── Update ─────────────────────────────
    public async Task<VendorProductDto> UpdateAsync(int vendorId, string productId, VendorProductWriteDto dto, CancellationToken ct = default)
    {
        var product = await GetOwnedProductOrThrowAsync(vendorId, productId, ct);

        product.Name = dto.Name.Trim();
        product.Subcategory = (dto.Category ?? string.Empty).ToLowerInvariant();
        product.Price = dto.Price;
        product.Image = dto.Image;
        product.Category = dto.Category;
        product.Gender = dto.Gender;
        product.Status = NormalizeStatus(dto.Status);
        product.InStock = dto.Stock > 0;

        _uow.Products.Update(product);

        // Replace variants entirely if Sizes/Colors changed.
        var existingVariants = (await _uow.ProductVariants.GetForProductAsync(productId, ct)).ToList();
        var existingKey = string.Join("|", existingVariants.Select(v => $"{v.Size}/{v.Color}").OrderBy(x => x));
        var newKey = string.Join("|",
            dto.Sizes.SelectMany(s => dto.Colors.Select(c => $"{s}/{c}")).OrderBy(x => x));

        if (existingKey != newKey)
        {
            foreach (var v in existingVariants) _uow.ProductVariants.Remove(v);
            var newVariants = BuildVariants(product, dto, vendorId);
            foreach (var v in newVariants) await _uow.ProductVariants.AddAsync(v, ct);
            await _uow.SaveChangesAsync(ct);
            return VendorMappers.ToProductDto(product, newVariants);
        }
        else
        {
            // Sizes/Colors unchanged. Distribute new total stock across existing variants.
            DistributeStock(existingVariants, dto.Stock);
            foreach (var v in existingVariants) _uow.ProductVariants.Update(v);
            await _uow.SaveChangesAsync(ct);
            return VendorMappers.ToProductDto(product, existingVariants);
        }
    }

    // ───────────────────────────── Delete ─────────────────────────────
    public async Task DeleteAsync(int vendorId, string productId, CancellationToken ct = default)
    {
        var product = await GetOwnedProductOrThrowAsync(vendorId, productId, ct);
        // Variants are removed via cascade FK at the DB level.
        _uow.Products.Remove(product);
        await _uow.SaveChangesAsync(ct);
    }

    // ───────────────────────────── Toggle status ─────────────────────────────
    public async Task<VendorProductDto> ToggleStatusAsync(int vendorId, string productId, CancellationToken ct = default)
    {
        var product = await GetOwnedProductOrThrowAsync(vendorId, productId, ct);

        var current = string.IsNullOrWhiteSpace(product.Status) ? "active" : product.Status!;
        product.Status = current.Equals("active", StringComparison.OrdinalIgnoreCase) ? "inactive" : "active";
        _uow.Products.Update(product);
        await _uow.SaveChangesAsync(ct);

        var variants = (await _uow.ProductVariants.GetForProductAsync(productId, ct)).ToList();
        return VendorMappers.ToProductDto(product, variants);
    }

    // ───────────────────────────── Helpers ─────────────────────────────
    private async Task<Product> GetOwnedProductOrThrowAsync(int vendorId, string productId, CancellationToken ct)
    {
        var product = await _uow.Products.GetByIdAsync(productId, ct)
            ?? throw new ApiException(404, "Product not found.", code: "NOT_FOUND");

        if (product.VendorId != vendorId)
            throw new ApiException(403, "You do not own this product.", code: "FORBIDDEN");

        return product;
    }

    private static string NormalizeStatus(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return "active";
        return raw.Equals("inactive", StringComparison.OrdinalIgnoreCase) ? "inactive" : "active";
    }

    private static string MakeSlug(string name)
    {
        var slug = (name ?? string.Empty).Trim().ToLowerInvariant();
        var clean = new System.Text.StringBuilder(slug.Length);
        foreach (var ch in slug)
        {
            if (char.IsLetterOrDigit(ch)) clean.Append(ch);
            else if (ch == ' ' || ch == '-' || ch == '_') clean.Append('-');
        }
        var s = clean.ToString().Trim('-');
        return string.IsNullOrEmpty(s) ? "product" : s;
    }

    private static List<ProductVariant> BuildVariants(Product product, VendorProductWriteDto dto, int vendorId)
    {
        // If FE supplied no sizes or no colors, fall back to a single "default" variant.
        var sizes = dto.Sizes is { Count: > 0 } ? dto.Sizes : new List<string> { "M" };
        var colors = dto.Colors is { Count: > 0 } ? dto.Colors : new List<string> { "Default" };

        var combos = new List<ProductVariant>();
        foreach (var s in sizes)
        {
            foreach (var c in colors)
            {
                combos.Add(new ProductVariant
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Size = s,
                    Color = c,
                    Stock = 0,
                    UpdatedAt = DateTime.UtcNow,
                    VendorId = vendorId
                });
            }
        }

        DistributeStock(combos, dto.Stock);
        return combos;
    }

    // Spread `total` across `variants` -- floor-divide, with the remainder
    // applied to the first variants so the sum exactly equals `total`.
    private static void DistributeStock(List<ProductVariant> variants, int total)
    {
        if (variants.Count == 0) return;
        var baseQty = total / variants.Count;
        var remainder = total % variants.Count;
        for (int i = 0; i < variants.Count; i++)
        {
            variants[i].Stock = baseQty + (i < remainder ? 1 : 0);
            variants[i].UpdatedAt = DateTime.UtcNow;
        }
    }
}
