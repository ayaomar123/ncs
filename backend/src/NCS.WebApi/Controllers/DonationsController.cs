using MediatR;
using Microsoft.AspNetCore.Mvc;
using NCS.Application.Features.Donations.Commands;

namespace NCS.WebApi.Controllers;

[ApiController]
[Route("api/donations")]
public sealed class DonationsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDonationRequestCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
