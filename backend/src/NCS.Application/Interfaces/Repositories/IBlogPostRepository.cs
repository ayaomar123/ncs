using NCS.Application.Common.Models;
using NCS.Domain.Entities;

namespace NCS.Application.Interfaces.Repositories;

public interface IBlogPostRepository
{
    Task<BlogPost?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<BlogPost?> GetBySlugAsync(string slug, bool includeUnpublished, CancellationToken cancellationToken);

    Task<PagedResult<BlogPost>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? tag,
        bool includeUnpublished,
        CancellationToken cancellationToken);

    Task AddAsync(BlogPost post, CancellationToken cancellationToken);
    Task UpdateAsync(BlogPost post, CancellationToken cancellationToken);
    Task DeleteAsync(BlogPost post, CancellationToken cancellationToken);

    Task<bool> SlugExistsAsync(string slug, Guid? excludeId, CancellationToken cancellationToken);
}
