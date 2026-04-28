using EcommerceLifestyle.BLL.Dtos.Users;

namespace EcommerceLifestyle.BLL.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetMeAsync(int userId, CancellationToken ct = default);
    Task<UserDto> UpdateMeAsync(int userId, UserUpdateDto dto, CancellationToken ct = default);

    // Admin-style CRUD (gated by feature flag in the controller).
    Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken ct = default);
    Task<UserDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
