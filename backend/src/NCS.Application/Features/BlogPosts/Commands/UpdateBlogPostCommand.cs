using MediatR;
using NCS.Application.Common.Exceptions;
using NCS.Application.Common.Slug;
using NCS.Application.Features.BlogPosts.Dtos;
using NCS.Application.Features.BlogPosts.Mappings;
using NCS.Application.Interfaces.Repositories;

namespace NCS.Application.Features.BlogPosts.Commands;

public sealed record UpdateBlogPostCommand(
    Guid Id,
    string Title,
    string? Slug,
    string Excerpt,
    string Content,
    string? CoverImageUrl,
    IReadOnlyList<string>? Tags,
    bool IsPublished) : IRequest<BlogPostDto>;

public sealed class UpdateBlogPostCommandHandler(IBlogPostRepository repository) : IRequestHandler<UpdateBlogPostCommand, BlogPostDto>
{
    public async Task<BlogPostDto> Handle(UpdateBlogPostCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
        {
            throw NotFoundException.For("BlogPost", request.Id);
        }

        if (!string.IsNullOrWhiteSpace(request.Slug))
        {
            var baseSlug = SlugGenerator.Generate(request.Slug);
            entity.Slug = await EnsureUniqueSlugAsync(baseSlug, entity.Id, cancellationToken);
        }

        entity.Title = request.Title.Trim();
        entity.Excerpt = request.Excerpt.Trim();
        entity.Content = request.Content.Trim();
        entity.CoverImageUrl = string.IsNullOrWhiteSpace(request.CoverImageUrl) ? null : request.CoverImageUrl.Trim();
        entity.Tags = request.Tags?.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList() ?? [];

        if (entity.IsPublished != request.IsPublished)
        {
            entity.IsPublished = request.IsPublished;

            if (request.IsPublished && entity.PublishedAt is null)
            {
                entity.PublishedAt = DateTimeOffset.UtcNow;
            }

            if (!request.IsPublished)
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
