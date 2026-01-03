using Microsoft.AspNetCore.Identity;
using NCS.Application.Interfaces.Auth;
using NCS.Domain.Entities;

namespace NCS.Infrastructure.Services;

public sealed class PasswordHasherService : IPasswordHasher
{
    private readonly PasswordHasher<AdminUser> _hasher = new();

    public string HashPassword(string password) =>
        _hasher.HashPassword(new AdminUser(), password);

    public bool VerifyHashedPassword(string passwordHash, string providedPassword) =>
        _hasher.VerifyHashedPassword(new AdminUser(), passwordHash, providedPassword) == PasswordVerificationResult.Success;
}
