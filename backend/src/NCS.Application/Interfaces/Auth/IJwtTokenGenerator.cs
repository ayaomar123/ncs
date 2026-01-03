namespace NCS.Application.Interfaces.Auth;

public interface IJwtTokenGenerator
{
    string GenerateAdminToken(string email);
}
