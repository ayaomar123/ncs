using MediatR;
using NCS.Application.Common.Slug;
using NCS.Application.Features.Appeals.Dtos;
using NCS.Application.Features.Appeals.Mappings;
using NCS.Application.Interfaces.Repositories;
using NCS.Domain.Entities;
using NCS.Domain.Enums;

namespace NCS.Application.Features.Appeals.Commands;

public sealed record CreateAppealCommand(
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

public sealed class CreateAppealCommandHandler(IAppealRepository repository) : IRequestHandler<CreateAppealCommand, AppealDto>
{
    public async Task<AppealDto> Handle(CreateAppealCommand request, CancellationToken cancellationToken)
    {
        var baseSlug = SlugGenerator.Generate(string.IsNullOrWhiteSpace(request.Slug) ? request.Title : request.Slug);
        var uniqueSlug = await EnsureUniqueSlugAsync(baseSlug, null, cancellationToken);

        var entity = new Appeal
        {
            Id = Guid.NewGuid(),
            Title = request.Title.Trim(),
            Slug = uniqueSlug,
            Summary = request.Summary.Trim(),
            Description = request.Description.Trim(),
            CountryTag = string.IsNullOrWhiteSpace(request.CountryTag) ? null : request.CountryTag.Trim(),
            IsUrgent = request.IsUrgent,
            TargetAmount = request.TargetAmount,
            RaisedAmount = request.RaisedAmount,
            CoverImageUrl = string.IsNullOrWhiteSpace(request.CoverImageUrl) ? null : request.CoverImageUrl.Trim(),
            GalleryUrls = request.GalleryUrls?.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList() ?? [],
            Status = request.Status,
            CreatedAt = DateTimeOffset.UtcNow,
            PublishedAt = request.Status == AppealStatus.Published ? DateTimeOffset.UtcNow : null
        };

        await repository.AddAsync(entity, cancellationToken);
        return entity.ToDto();
    }

    private async Task<string> EnsureUniqueSlugAsync(string baseSlug, Guid? excludeId, CancellationToken cancellationToken)
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
