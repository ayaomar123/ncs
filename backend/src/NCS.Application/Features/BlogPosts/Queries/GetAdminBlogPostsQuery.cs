using MediatR;
using NCS.Application.Common.Models;
using NCS.Application.Features.BlogPosts.Dtos;
using NCS.Application.Features.BlogPosts.Mappings;
using NCS.Application.Interfaces.Repositories;

namespace NCS.Application.Features.BlogPosts.Queries;

public sealed record GetAdminBlogPostsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? Tag = null) : IRequest<PagedResult<BlogPostDto>>;

public sealed class GetAdminBlogPostsQueryHandler(IBlogPostRepository repository) : IRequestHandler<GetAdminBlogPostsQuery, PagedResult<BlogPostDto>>
{
    public async Task<PagedResult<BlogPostDto>> Handle(GetAdminBlogPostsQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            request.Tag,
            includeUnpublished: true,
            cancellationToken);

        return new PagedResult<BlogPostDto>(
            result.Items.Select(x => x.ToDto()).ToList(),
            result.PageNumber,
            result.PageSize,
            result.TotalCount);
    }
}
