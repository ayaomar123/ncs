using NCS.Domain.Entities;

namespace NCS.Application.Interfaces.Repositories;

public interface IAdminUserRepository
{
    Task<AdminUser?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<bool> AnyAsync(CancellationToken cancellationToken);
    Task AddAsync(AdminUser user, CancellationToken cancellationToken);
}
