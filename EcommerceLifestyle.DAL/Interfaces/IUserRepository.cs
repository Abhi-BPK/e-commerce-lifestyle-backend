using EcommerceLifestyle.DAL.Entities;

namespace EcommerceLifestyle.DAL.Interfaces;

public interface IUserRepository : IRepository<User, int>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);

    // Used by external (OIDC) login to find the user by the stable provider id
    // (Google "sub" / GitHub user id). Returns null when the user has not yet
    // been linked or created.
    Task<User?> GetByExternalLoginAsync(string provider, string providerUserId, CancellationToken ct = default);
}
