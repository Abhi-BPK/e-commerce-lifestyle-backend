using EcommerceLifestyle.BLL.Dtos.Logs;
using EcommerceLifestyle.BLL.Interfaces;
using EcommerceLifestyle.BLL.Mapping;
using EcommerceLifestyle.DAL.Entities;
using EcommerceLifestyle.DAL.Interfaces;

namespace EcommerceLifestyle.BLL.Services;

public class LogService : ILogService
{
    private readonly IUnitOfWork _uow;

    public LogService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<LogDto>> QueryAsync(string? level, DateTime? since, int take, CancellationToken ct = default)
    {
        var rows = await _uow.Logs.GetRecentAsync(level, since, take, ct);
        return rows.Select(DtoMappers.ToDto);
    }

    public async Task<LogDto?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        var l = await _uow.Logs.GetByIdAsync(id, ct);
        return l is null ? null : DtoMappers.ToDto(l);
    }

    public async Task WriteAsync(Log entry, CancellationToken ct = default)
    {
        await _uow.Logs.AddAsync(entry, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
