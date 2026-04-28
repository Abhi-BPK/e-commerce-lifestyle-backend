using EcommerceLifestyle.BLL.Interfaces;

namespace EcommerceLifestyle.BLL.Services;

// BCrypt is salted + slow-by-design (work factor) -- the industry standard
// for password hashing. Never store raw passwords.
public class BcryptPasswordHasher : IPasswordHasher
{
    public string Hash(string raw) => BCrypt.Net.BCrypt.HashPassword(raw);

    public bool Verify(string raw, string hash) => BCrypt.Net.BCrypt.Verify(raw, hash);
}
