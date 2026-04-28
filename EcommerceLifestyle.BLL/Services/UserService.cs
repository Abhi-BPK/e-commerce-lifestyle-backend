using EcommerceLifestyle.BLL.Dtos.Users;
using EcommerceLifestyle.BLL.Exceptions;
using EcommerceLifestyle.BLL.Interfaces;
using EcommerceLifestyle.BLL.Mapping;
using EcommerceLifestyle.DAL.Interfaces;

namespace EcommerceLifestyle.BLL.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _uow;

    public UserService(IUnitOfWork uow) => _uow = uow;

    public async Task<UserDto?> GetMeAsync(int userId, CancellationToken ct = default)
    {
        var u = await _uow.Users.GetByIdAsync(userId, ct);
        return u is null ? null : DtoMappers.ToUserDto(u);
    }

    public async Task<UserDto> UpdateMeAsync(int userId, UserUpdateDto dto, CancellationToken ct = default)
    {
        var u = await _uow.Users.GetByIdAsync(userId, ct)
            ?? throw new ApiException(404, "User not found.", code: "NOT_FOUND");

        u.FirstName = dto.FirstName.Trim();
        u.LastName = dto.LastName.Trim();
        _uow.Users.Update(u);
        await _uow.SaveChangesAsync(ct);
        return DtoMappers.ToUserDto(u);
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken ct = default)
    {
        var users = await _uow.Users.GetAllAsync(ct);
        return users.Select(DtoMappers.ToUserDto);
    }

    public async Task<UserDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var u = await _uow.Users.GetByIdAsync(id, ct);
        return u is null ? null : DtoMappers.ToUserDto(u);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var u = await _uow.Users.GetByIdAsync(id, ct)
            ?? throw new ApiException(404, "User not found.", code: "NOT_FOUND");
        _uow.Users.Remove(u);
        await _uow.SaveChangesAsync(ct);
    }
}
