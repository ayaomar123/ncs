using NCS.Application.Common.Models;
using NCS.Domain.Entities;

namespace NCS.Application.Interfaces.Repositories;

public interface IAppealRepository
{
    Task<Appeal?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Appeal?> GetBySlugAsync(string slug, CancellationToken cancellationToken);

    Task<PagedResult<Appeal>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        bool? isUrgent,
        string? countryTag,
        bool includeUnpublished,
        CancellationToken cancellationToken);

    Task AddAsync(Appeal appeal, CancellationToken cancellationToken);
    Task UpdateAsync(Appeal appeal, CancellationToken cancellationToken);
    Task DeleteAsync(Appeal appeal, CancellationToken cancellationToken);

    Task<bool> SlugExistsAsync(string slug, Guid? excludeId, CancellationToken cancellationToken);
}
