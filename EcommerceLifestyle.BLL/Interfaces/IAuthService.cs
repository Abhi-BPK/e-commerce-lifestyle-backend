using EcommerceLifestyle.BLL.Dtos.Auth;

namespace EcommerceLifestyle.BLL.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest req, CancellationToken ct = default);
    Task SignupAsync(SignupRequest req, CancellationToken ct = default);
}
