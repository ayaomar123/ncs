using NCS.Application.Features.Appeals.Dtos;
using NCS.Domain.Entities;

namespace NCS.Application.Features.Appeals.Mappings;

public static class AppealMappings
{
    public static AppealDto ToDto(this Appeal appeal) =>
        new(
            appeal.Id,
            appeal.Title,
            appeal.Slug,
            appeal.Summary,
            appeal.Description,
            appeal.CountryTag,
            appeal.IsUrgent,
            appeal.TargetAmount,
            appeal.RaisedAmount,
            appeal.CoverImageUrl,
            appeal.GalleryUrls,
            appeal.Status,
            appeal.CreatedAt,
            appeal.PublishedAt);
}
