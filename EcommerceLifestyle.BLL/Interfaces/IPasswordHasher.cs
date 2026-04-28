namespace EcommerceLifestyle.BLL.Interfaces;

// Tiny abstraction so the AuthService doesn't depend directly on BCrypt.
// Lets us swap implementations (e.g. unit-test a plain-text hasher) easily.
public interface IPasswordHasher
{
    string Hash(string raw);
    bool Verify(string raw, string hash);
}
