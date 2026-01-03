using NCS.Application.Features.BlogPosts.Dtos;
using NCS.Domain.Entities;

namespace NCS.Application.Features.BlogPosts.Mappings;

public static class BlogPostMappings
{
    public static BlogPostDto ToDto(this BlogPost post) =>
        new(
            post.Id,
            post.Title,
            post.Slug,
            post.Excerpt,
            post.Content,
            post.CoverImageUrl,
            post.Tags,
            post.CreatedAt,
            post.PublishedAt,
            post.IsPublished);
}
