using EcommerceLifestyle.BLL.Dtos.Vendor;
using EcommerceLifestyle.BLL.Exceptions;
using EcommerceLifestyle.BLL.Interfaces;
using EcommerceLifestyle.BLL.Mapping;
using EcommerceLifestyle.DAL.Interfaces;

namespace EcommerceLifestyle.BLL.Services;

public class VendorInventoryService : IVendorInventoryService
{
    private readonly IUnitOfWork _uow;

    public VendorInventoryService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<VendorInventoryDto>> GetForVendorAsync(int vendorId, CancellationToken ct = default)
    {
        var variants = await _uow.ProductVariants.GetForVendorAsync(vendorId, ct);
        return variants.Select(VendorMappers.ToInventoryDto);
    }

    public async Task<VendorInventoryDto> UpdateStockAsync(int vendorId, int variantId, int newStock, CancellationToken ct = default)
    {
        if (newStock < 0)
            throw new ApiException(400, "Stock must be >= 0.", field: "stock", code: "VALIDATION");

        var variant = await _uow.ProductVariants.GetByIdAsync(variantId, ct)
            ?? throw new ApiException(404, "Inventory variant not found.", code: "NOT_FOUND");

        if (variant.VendorId != vendorId)
            throw new ApiException(403, "You do not own this inventory row.", code: "FORBIDDEN");

        variant.Stock = newStock;
        variant.UpdatedAt = DateTime.UtcNow;
        _uow.ProductVariants.Update(variant);
        await _uow.SaveChangesAsync(ct);

        return VendorMappers.ToInventoryDto(variant);
    }
}
