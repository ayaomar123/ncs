using MediatR;
using Microsoft.AspNetCore.Mvc;
using NCS.Application.Features.BlogPosts.Queries;

namespace NCS.WebApi.Controllers;

[ApiController]
[Route("api/posts")]
public sealed class PostsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetBlogPostsQuery query, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug([FromRoute] string slug, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetBlogPostBySlugQuery(slug), cancellationToken);
        return Ok(result);
    }
}
