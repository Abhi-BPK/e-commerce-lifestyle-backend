using EcommerceLifestyle.DAL.Entities;

namespace EcommerceLifestyle.BLL.Interfaces;

public interface ITokenService
{
    // Issues a signed JWT with claims for sub (user id), email, and role.
    string Create(User user);
}
