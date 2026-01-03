using MediatR;
using Microsoft.AspNetCore.Mvc;
using NCS.Application.Features.Contact.Commands;

namespace NCS.WebApi.Controllers;

[ApiController]
[Route("api/contact")]
public sealed class ContactController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContactMessageCommand command, CancellationToken cancellationToken)
    {
        var id = await mediator.Send(command, cancellationToken);
        return Ok(new { id });
    }
}
