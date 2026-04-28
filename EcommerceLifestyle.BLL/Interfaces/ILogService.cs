using EcommerceLifestyle.BLL.Dtos.Logs;
using EcommerceLifestyle.DAL.Entities;

namespace EcommerceLifestyle.BLL.Interfaces;

public interface ILogService
{
    Task<IEnumerable<LogDto>> QueryAsync(string? level, DateTime? since, int take, CancellationToken ct = default);
    Task<LogDto?> GetByIdAsync(long id, CancellationToken ct = default);

    // Used by middleware. Takes the raw entity to avoid extra mapping in hot path.
    Task WriteAsync(Log entry, CancellationToken ct = default);
}
