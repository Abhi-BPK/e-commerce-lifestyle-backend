using EcommerceLifestyle.DAL.Entities;

namespace EcommerceLifestyle.DAL.Interfaces;

public interface ILogRepository : IRepository<Log, long>
{
    Task<IEnumerable<Log>> GetRecentAsync(string? level, DateTime? since, int take, CancellationToken ct = default);
}
