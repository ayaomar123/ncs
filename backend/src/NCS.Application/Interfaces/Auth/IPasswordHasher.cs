namespace NCS.Application.Interfaces.Auth;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyHashedPassword(string passwordHash, string providedPassword);
}
