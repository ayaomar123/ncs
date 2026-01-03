using MediatR;
using Microsoft.AspNetCore.Mvc;
using NCS.Application.Features.Appeals.Queries;

namespace NCS.WebApi.Controllers;

[ApiController]
[Route("api/appeals")]
public sealed class AppealsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAppealsQuery query, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug([FromRoute] string slug, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAppealBySlugQuery(slug), cancellationToken);
        return Ok(result);
    }
}
