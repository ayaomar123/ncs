using Microsoft.EntityFrameworkCore;
using NCS.Application.Common.Models;
using NCS.Application.Interfaces.Repositories;
using NCS.Domain.Entities;
using NCS.Domain.Enums;
using NCS.Infrastructure.Persistence;

namespace NCS.Infrastructure.Repositories;

public sealed class AppealRepository(NcsDbContext db) : IAppealRepository
{
    public Task<Appeal?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.Appeals.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Appeal?> GetBySlugAsync(string slug, CancellationToken cancellationToken) =>
        db.Appeals.FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken);

    public async Task<PagedResult<Appeal>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        bool? isUrgent,
        string? countryTag,
        bool includeUnpublished,
        CancellationToken cancellationToken)
    {
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize is < 1 or > 100 ? 10 : pageSize;

        var query = db.Appeals.AsNoTracking().AsQueryable();

        if (!includeUnpublished)
        {
            query = query.Where(x => x.Status == AppealStatus.Published);
        }

        if (isUrgent is not null)
        {
            query = query.Where(x => x.IsUrgent == isUrgent);
        }

        if (!string.IsNullOrWhiteSpace(countryTag))
        {
            query = query.Where(x => x.CountryTag == countryTag);
        }

        query = query.OrderByDescending(x => x.IsUrgent).ThenByDescending(x => x.PublishedAt ?? x.CreatedAt);

        var total = await query.LongCountAsync(cancellationToken);
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Appeal>(items, pageNumber, pageSize, total);
    }

    public async Task AddAsync(Appeal appeal, CancellationToken cancellationToken)
    {
        db.Appeals.Add(appeal);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Appeal appeal, CancellationToken cancellationToken)
    {
        db.Appeals.Update(appeal);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Appeal appeal, CancellationToken cancellationToken)
    {
        db.Appeals.Remove(appeal);
        await db.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> SlugExistsAsync(string slug, Guid? excludeId, CancellationToken cancellationToken)
    {
        var query = db.Appeals.AsQueryable().Where(x => x.Slug == slug);

        if (excludeId is not null)
        {
            query = query.Where(x => x.Id != excludeId);
        }

        return query.AnyAsync(cancellationToken);
    }
}
