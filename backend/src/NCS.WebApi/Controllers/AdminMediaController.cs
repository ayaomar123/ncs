using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NCS.Application.Interfaces.Storage;

namespace NCS.WebApi.Controllers;

public sealed class UploadFileRequest
{
    public required IFormFile File { get; set; }
}

[ApiController]
[Authorize(Policy = "Admin")]
[Route("api/admin/media")]
public sealed class AdminMediaController(IFileStorage fileStorage) : ControllerBase
{
    [HttpPost("upload")]
    [RequestSizeLimit(10_000_000)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload([FromForm] UploadFileRequest request, CancellationToken cancellationToken)
    {
        if (request.File.Length == 0)
        {
            return BadRequest(new { message = "File is empty" });
        }

        await using var stream = request.File.OpenReadStream();
        var url = await fileStorage.SaveAsync(stream, request.File.FileName, request.File.ContentType, cancellationToken);
        return Ok(new { url });
    }
}
