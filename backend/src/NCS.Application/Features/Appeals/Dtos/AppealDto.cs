using NCS.Domain.Enums;

namespace NCS.Application.Features.Appeals.Dtos;

public sealed record AppealDto(
    Guid Id,
    string Title,
    string Slug,
    string Summary,
    string Description,
    string? CountryTag,
    bool IsUrgent,
    decimal TargetAmount,
    decimal RaisedAmount,
    string? CoverImageUrl,
    IReadOnlyList<string> GalleryUrls,
    AppealStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? PublishedAt);
