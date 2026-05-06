using EcommerceLifestyle.DAL.Entities;
using EcommerceLifestyle.DAL.Interfaces;
using EcommerceLifestyle.DAL.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EcommerceLifestyle.DAL.Repositories;

public class UserRepository : Repository<User, int>, IUserRepository
{
    public UserRepository(AppDbContext db) : base(db) { }

    // Email is unique -- there is only ever zero or one match.
    public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        => _set.FirstOrDefaultAsync(u => u.Email == email, ct);

    public Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
        => _set.AnyAsync(u => u.Email == email, ct);

    public Task<User?> GetByExternalLoginAsync(string provider, string providerUserId, CancellationToken ct = default)
        => _set.FirstOrDefaultAsync(u => u.Provider == provider && u.ProviderUserId == providerUserId, ct);
}
