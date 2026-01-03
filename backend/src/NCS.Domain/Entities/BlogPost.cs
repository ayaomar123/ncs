using NCS.Domain.Common;

namespace NCS.Domain.Entities;

public class BlogPost : Entity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public string Excerpt { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string? CoverImageUrl { get; set; }

    public List<string> Tags { get; set; } = [];

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? PublishedAt { get; set; }

    public bool IsPublished { get; set; }
}
