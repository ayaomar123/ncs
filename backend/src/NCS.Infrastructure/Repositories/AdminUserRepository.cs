using Microsoft.EntityFrameworkCore;
using NCS.Application.Interfaces.Repositories;
using NCS.Domain.Entities;
using NCS.Infrastructure.Persistence;

namespace NCS.Infrastructure.Repositories;

public sealed class AdminUserRepository(NcsDbContext db) : IAdminUserRepository
{
    public Task<AdminUser?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        db.AdminUsers.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

    public Task<bool> AnyAsync(CancellationToken cancellationToken) =>
        db.AdminUsers.AnyAsync(cancellationToken);

    public async Task AddAsync(AdminUser user, CancellationToken cancellationToken)
    {
        db.AdminUsers.Add(user);
        await db.SaveChangesAsync(cancellationToken);
    }
}
