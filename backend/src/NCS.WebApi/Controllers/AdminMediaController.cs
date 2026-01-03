using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NCS.Application.Interfaces.Storage;

namespace NCS.WebApi.Controllers;

[ApiController]
[Authorize(Policy = "Admin")]
[Route("api/admin/media")]
public sealed class AdminMediaController(IFileStorage fileStorage) : ControllerBase
{
    [HttpPost("upload")]
    [RequestSizeLimit(10_000_000)]
    public async Task<IActionResult> Upload([FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        if (file.Length == 0)
        {
            return BadRequest(new { message = "File is empty" });
        }

        await using var stream = file.OpenReadStream();
        var url = await fileStorage.SaveAsync(stream, file.FileName, file.ContentType, cancellationToken);
        return Ok(new { url });
    }
}
