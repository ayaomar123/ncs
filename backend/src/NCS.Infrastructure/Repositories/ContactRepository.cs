using NCS.Application.Interfaces.Repositories;
using NCS.Domain.Entities;
using NCS.Infrastructure.Persistence;

namespace NCS.Infrastructure.Repositories;

public sealed class ContactRepository(NcsDbContext db) : IContactRepository
{
    public async Task AddAsync(ContactMessage message, CancellationToken cancellationToken)
    {
        db.ContactMessages.Add(message);
        await db.SaveChangesAsync(cancellationToken);
    }
}
