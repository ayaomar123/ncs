using MediatR;
using NCS.Application.Common.Exceptions;
using NCS.Application.Features.BlogPosts.Dtos;
using NCS.Application.Features.BlogPosts.Mappings;
using NCS.Application.Interfaces.Repositories;

namespace NCS.Application.Features.BlogPosts.Queries;

public sealed record GetBlogPostBySlugQuery(string Slug) : IRequest<BlogPostDto>;

public sealed class GetBlogPostBySlugQueryHandler(IBlogPostRepository repository) : IRequestHandler<GetBlogPostBySlugQuery, BlogPostDto>
{
    public async Task<BlogPostDto> Handle(GetBlogPostBySlugQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetBySlugAsync(request.Slug, includeUnpublished: false, cancellationToken);
        if (entity is null || !entity.IsPublished)
        {
            throw NotFoundException.For("BlogPost", request.Slug);
        }

        return entity.ToDto();
    }
}
