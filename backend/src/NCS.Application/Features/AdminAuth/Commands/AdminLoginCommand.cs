using MediatR;
using NCS.Application.Common.Exceptions;
using NCS.Application.Features.AdminAuth.Dtos;
using NCS.Application.Interfaces.Auth;
using NCS.Application.Interfaces.Repositories;

namespace NCS.Application.Features.AdminAuth.Commands;

public sealed record AdminLoginCommand(string Email, string Password) : IRequest<AdminLoginResponseDto>;

public sealed class AdminLoginCommandHandler(
    IAdminUserRepository repository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<AdminLoginCommand, AdminLoginResponseDto>
{
    public async Task<AdminLoginResponseDto> Handle(AdminLoginCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetByEmailAsync(request.Email.Trim(), cancellationToken);
        if (user is null)
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        if (!passwordHasher.VerifyHashedPassword(user.PasswordHash, request.Password))
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        var token = jwtTokenGenerator.GenerateAdminToken(user.Email);
        return new AdminLoginResponseDto(token);
    }
}
