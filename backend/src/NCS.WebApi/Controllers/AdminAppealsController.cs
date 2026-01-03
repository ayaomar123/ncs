using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NCS.Application.Features.Appeals.Commands;
using NCS.Application.Features.Appeals.Queries;

namespace NCS.WebApi.Controllers;

[ApiController]
[Authorize(Policy = "Admin")]
[Route("api/admin/appeals")]
public sealed class AdminAppealsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAdminAppealsQuery query, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAppealByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppealCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAppealCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest(new { message = "Route id does not match payload id" });
        }

        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteAppealCommand(id), cancellationToken);
        return NoContent();
    }
}
