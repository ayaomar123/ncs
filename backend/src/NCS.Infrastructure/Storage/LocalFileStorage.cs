using Microsoft.Extensions.Options;
using NCS.Application.Interfaces.Storage;

namespace NCS.Infrastructure.Storage;

public sealed class LocalFileStorage(IOptions<LocalFileStorageOptions> options) : IFileStorage
{
    private readonly LocalFileStorageOptions _options = options.Value;

    public async Task<string> SaveAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(_options.RootPath);

        var safeFileName = Path.GetFileName(fileName);
        var ext = Path.GetExtension(safeFileName);
        var storedFileName = $"{Guid.NewGuid():N}{ext}";
        var fullPath = Path.Combine(_options.RootPath, storedFileName);

        await using var fileStream = File.Create(fullPath);
        await content.CopyToAsync(fileStream, cancellationToken);

        return $"{_options.PublicUrlPrefix.TrimEnd('/')}/{storedFileName}";
    }
}
