using MediatR;
using Microsoft.AspNetCore.Mvc;
using NCS.Application.Features.AdminAuth.Commands;

namespace NCS.WebApi.Controllers;

[ApiController]
[Route("api/admin/auth")]
public sealed class AdminAuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AdminLoginCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
