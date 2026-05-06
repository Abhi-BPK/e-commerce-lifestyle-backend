using EcommerceLifestyle.BLL.Dtos.Auth;
using EcommerceLifestyle.BLL.Exceptions;
using EcommerceLifestyle.BLL.Interfaces;
using EcommerceLifestyle.BLL.Mapping;
using EcommerceLifestyle.DAL.Entities;
using EcommerceLifestyle.DAL.Entities.Enums;
using EcommerceLifestyle.DAL.Interfaces;

namespace EcommerceLifestyle.BLL.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokens;

    public AuthService(IUnitOfWork uow, IPasswordHasher hasher, ITokenService tokens)
    {
        _uow = uow;
        _hasher = hasher;
        _tokens = tokens;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest req, CancellationToken ct = default)
    {
        var user = await _uow.Users.GetByEmailAsync(req.Email.Trim().ToLowerInvariant(), ct);
        // External-login accounts (Google/GitHub) have a NULL PasswordHash --
        // they must sign in via /api/auth/oidc/{provider}, never with a password.
        if (user is null || string.IsNullOrEmpty(user.PasswordHash) || !_hasher.Verify(req.Password, user.PasswordHash))
        {
            // Same message either way -- don't leak which half is wrong.
            throw new ApiException(401, "Invalid email or password.", code: "INVALID_CREDENTIALS");
        }

        return new LoginResponse
        {
            Token = _tokens.Create(user),
            User = DtoMappers.ToAuthUserDto(user)
        };
    }

    public async Task SignupAsync(SignupRequest req, CancellationToken ct = default)
    {
        var email = req.Email.Trim().ToLowerInvariant();

        if (await _uow.Users.EmailExistsAsync(email, ct))
            throw new ApiException(409, "Email already registered.", field: "email", code: "EMAIL_EXISTS");

        // Default to "User" if missing/blank/unknown -- prevents privilege escalation by typo.
        var role = UserRole.User;
        if (!string.IsNullOrWhiteSpace(req.Role) &&
            Enum.TryParse<UserRole>(req.Role, ignoreCase: true, out var parsed))
        {
            role = parsed;
        }

        var user = new User
        {
            FirstName = req.FirstName.Trim(),
            LastName = req.LastName.Trim(),
            Email = email,
            PasswordHash = _hasher.Hash(req.Password),
            Role = role,
            CreatedAt = DateTime.UtcNow
        };

        await _uow.Users.AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
