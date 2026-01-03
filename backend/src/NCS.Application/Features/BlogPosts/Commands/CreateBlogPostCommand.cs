using MediatR;
using NCS.Application.Common.Slug;
using NCS.Application.Features.BlogPosts.Dtos;
using NCS.Application.Features.BlogPosts.Mappings;
using NCS.Application.Interfaces.Repositories;
using NCS.Domain.Entities;

namespace NCS.Application.Features.BlogPosts.Commands;

public sealed record CreateBlogPostCommand(
    string Title,
    string? Slug,
    string Excerpt,
    string Content,
    string? CoverImageUrl,
    IReadOnlyList<string>? Tags,
    bool IsPublished) : IRequest<BlogPostDto>;

public sealed class CreateBlogPostCommandHandler(IBlogPostRepository repository) : IRequestHandler<CreateBlogPostCommand, BlogPostDto>
{
    public async Task<BlogPostDto> Handle(CreateBlogPostCommand request, CancellationToken cancellationToken)
    {
        var baseSlug = SlugGenerator.Generate(string.IsNullOrWhiteSpace(request.Slug) ? request.Title : request.Slug);
        var uniqueSlug = await EnsureUniqueSlugAsync(baseSlug, null, cancellationToken);

        var entity = new BlogPost
        {
            Id = Guid.NewGuid(),
            Title = request.Title.Trim(),
            Slug = uniqueSlug,
            Excerpt = request.Excerpt.Trim(),
            Content = request.Content.Trim(),
            CoverImageUrl = string.IsNullOrWhiteSpace(request.CoverImageUrl) ? null : request.CoverImageUrl.Trim(),
            Tags = request.Tags?.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList() ?? [],
            CreatedAt = DateTimeOffset.UtcNow,
            IsPublished = request.IsPublished,
            PublishedAt = request.IsPublished ? DateTimeOffset.UtcNow : null
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
