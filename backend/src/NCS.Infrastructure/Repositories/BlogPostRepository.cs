using Microsoft.EntityFrameworkCore;
using NCS.Application.Common.Models;
using NCS.Application.Interfaces.Repositories;
using NCS.Domain.Entities;
using NCS.Infrastructure.Persistence;

namespace NCS.Infrastructure.Repositories;

public sealed class BlogPostRepository(NcsDbContext db) : IBlogPostRepository
{
    public Task<BlogPost?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.BlogPosts.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<BlogPost?> GetBySlugAsync(string slug, bool includeUnpublished, CancellationToken cancellationToken)
    {
        var query = db.BlogPosts.AsQueryable().Where(x => x.Slug == slug);
        if (!includeUnpublished)
        {
            query = query.Where(x => x.IsPublished);
        }

        return query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResult<BlogPost>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? tag,
        bool includeUnpublished,
        CancellationToken cancellationToken)
    {
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize is < 1 or > 100 ? 10 : pageSize;

        var query = db.BlogPosts.AsNoTracking().AsQueryable();

        if (!includeUnpublished)
        {
            query = query.Where(x => x.IsPublished);
        }

        if (!string.IsNullOrWhiteSpace(tag))
        {
            var tagLower = tag.Trim().ToLowerInvariant();
            query = query.Where(x => x.Tags.Any(t => t.ToLower() == tagLower));
        }

        query = query.OrderByDescending(x => x.PublishedAt ?? x.CreatedAt);

        var total = await query.LongCountAsync(cancellationToken);
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<BlogPost>(items, pageNumber, pageSize, total);
    }

    public async Task AddAsync(BlogPost post, CancellationToken cancellationToken)
    {
        db.BlogPosts.Add(post);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(BlogPost post, CancellationToken cancellationToken)
    {
        db.BlogPosts.Update(post);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(BlogPost post, CancellationToken cancellationToken)
    {
        db.BlogPosts.Remove(post);
        await db.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> SlugExistsAsync(string slug, Guid? excludeId, CancellationToken cancellationToken)
    {
        var query = db.BlogPosts.AsQueryable().Where(x => x.Slug == slug);

        if (excludeId is not null)
        {
            query = query.Where(x => x.Id != excludeId);
        }

        return query.AnyAsync(cancellationToken);
    }
}
