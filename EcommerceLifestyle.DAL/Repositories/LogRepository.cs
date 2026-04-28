using EcommerceLifestyle.DAL.Entities;
using EcommerceLifestyle.DAL.Interfaces;
using EcommerceLifestyle.DAL.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EcommerceLifestyle.DAL.Repositories;

public class LogRepository : Repository<Log, long>, ILogRepository
{
    public LogRepository(AppDbContext db) : base(db) { }

    public async Task<IEnumerable<Log>> GetRecentAsync(string? level, DateTime? since, int take, CancellationToken ct = default)
    {
        var q = _set.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(level))
            q = q.Where(l => l.Level == level);

        if (since.HasValue)
            q = q.Where(l => l.Timestamp >= since.Value);

        return await q.OrderByDescending(l => l.Timestamp)
                      .Take(take <= 0 ? 100 : take)
                      .ToListAsync(ct);
    }
}
