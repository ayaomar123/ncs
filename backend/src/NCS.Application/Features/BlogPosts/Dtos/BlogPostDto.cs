namespace NCS.Application.Features.BlogPosts.Dtos;

public sealed record BlogPostDto(
    Guid Id,
    string Title,
    string Slug,
    string Excerpt,
    string Content,
    string? CoverImageUrl,
    IReadOnlyList<string> Tags,
    DateTimeOffset CreatedAt,
    DateTimeOffset? PublishedAt,
    bool IsPublished);
