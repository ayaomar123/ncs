using MediatR;
using NCS.Application.Common.Exceptions;
using NCS.Application.Features.BlogPosts.Dtos;
using NCS.Application.Features.BlogPosts.Mappings;
using NCS.Application.Interfaces.Repositories;

namespace NCS.Application.Features.BlogPosts.Queries;

public sealed record GetBlogPostByIdQuery(Guid Id) : IRequest<BlogPostDto>;

public sealed class GetBlogPostByIdQueryHandler(IBlogPostRepository repository) : IRequestHandler<GetBlogPostByIdQuery, BlogPostDto>
{
    public async Task<BlogPostDto> Handle(GetBlogPostByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
        {
            throw NotFoundException.For("BlogPost", request.Id);
        }

        return entity.ToDto();
    }
}
