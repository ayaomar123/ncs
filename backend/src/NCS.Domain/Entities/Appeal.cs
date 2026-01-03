using NCS.Domain.Common;
using NCS.Domain.Enums;

namespace NCS.Domain.Entities;

public class Appeal : Entity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string? CountryTag { get; set; }
    public bool IsUrgent { get; set; }

    public decimal TargetAmount { get; set; }
    public decimal RaisedAmount { get; set; }

    public string? CoverImageUrl { get; set; }
    public List<string> GalleryUrls { get; set; } = [];

    public AppealStatus Status { get; set; } = AppealStatus.Draft;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? PublishedAt { get; set; }
}
