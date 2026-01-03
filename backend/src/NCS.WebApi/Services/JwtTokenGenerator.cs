using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NCS.Application.Interfaces.Auth;
using NCS.WebApi.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NCS.WebApi.Services;

public sealed class JwtTokenGenerator(IOptions<JwtOptions> options) : IJwtTokenGenerator
{
    private readonly JwtOptions _options = options.Value;

    public string GenerateAdminToken(string email)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, email),
            new(JwtRegisteredClaimNames.Email, email),
            new(ClaimTypes.Role, "Admin")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
