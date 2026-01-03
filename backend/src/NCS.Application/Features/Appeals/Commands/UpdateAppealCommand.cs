using MediatR;
using NCS.Application.Common.Exceptions;
using NCS.Application.Common.Slug;
using NCS.Application.Features.Appeals.Dtos;
using NCS.Application.Features.Appeals.Mappings;
using NCS.Application.Interfaces.Repositories;
using NCS.Domain.Enums;

namespace NCS.Application.Features.Appeals.Commands;

public sealed record UpdateAppealCommand(
    Guid Id,
    string Title,
    string? Slug,
    string Summary,
    string Description,
    string? CountryTag,
    bool IsUrgent,
    decimal TargetAmount,
    decimal RaisedAmount,
    string? CoverImageUrl,
    IReadOnlyList<string>? GalleryUrls,
    AppealStatus Status) : IRequest<AppealDto>;

public sealed class UpdateAppealCommandHandler(IAppealRepository repository) : IRequestHandler<UpdateAppealCommand, AppealDto>
{
    public async Task<AppealDto> Handle(UpdateAppealCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
        {
            throw NotFoundException.For("Appeal", request.Id);
        }

        if (!string.IsNullOrWhiteSpace(request.Slug))
        {
            var baseSlug = SlugGenerator.Generate(request.Slug);
            entity.Slug = await EnsureUniqueSlugAsync(baseSlug, entity.Id, cancellationToken);
        }

        entity.Title = request.Title.Trim();
        entity.Summary = request.Summary.Trim();
        entity.Description = request.Description.Trim();
        entity.CountryTag = string.IsNullOrWhiteSpace(request.CountryTag) ? null : request.CountryTag.Trim();
        entity.IsUrgent = request.IsUrgent;
        entity.TargetAmount = request.TargetAmount;
        entity.RaisedAmount = request.RaisedAmount;
        entity.CoverImageUrl = string.IsNullOrWhiteSpace(request.CoverImageUrl) ? null : request.CoverImageUrl.Trim();
        entity.GalleryUrls = request.GalleryUrls?.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList() ?? [];

        if (entity.Status != request.Status)
        {
            entity.Status = request.Status;

            if (request.Status == AppealStatus.Published && entity.PublishedAt is null)
            {
                entity.PublishedAt = DateTimeOffset.UtcNow;
            }

            if (request.Status != AppealStatus.Published)
            {
                entity.PublishedAt = null;
            }
        }

        await repository.UpdateAsync(entity, cancellationToken);
        return entity.ToDto();
    }

    private async Task<string> EnsureUniqueSlugAsync(string baseSlug, Guid excludeId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(baseSlug))
        {
            baseSlug = Guid.NewGuid().ToString("N");
        }

        var slug = baseSlug;
        var suffix = 2;

        while (await repository.SlugExistsAsync(slug, excludeId, cancellationToken))
        {
            slug = $"{baseSlug}-{suffix}";
            suffix++;
        }

        return slug;
    }
}
