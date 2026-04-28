using EcommerceLifestyle.BLL.Dtos.Inventory;

namespace EcommerceLifestyle.BLL.Interfaces;

public interface IInventoryService
{
    Task<IEnumerable<InventoryDto>> GetAllAsync(CancellationToken ct = default);
    Task<InventoryDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<InventoryDto?> GetByProductIdAsync(string productId, CancellationToken ct = default);
    Task<InventoryDto> CreateAsync(InventoryWriteDto dto, int? vendorId, CancellationToken ct = default);
    Task<InventoryDto> UpdateAsync(int id, InventoryWriteDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
