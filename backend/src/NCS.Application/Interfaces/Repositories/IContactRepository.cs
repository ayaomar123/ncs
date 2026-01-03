using NCS.Domain.Entities;

namespace NCS.Application.Interfaces.Repositories;

public interface IContactRepository
{
    Task AddAsync(ContactMessage message, CancellationToken cancellationToken);
}
