using System.ComponentModel.DataAnnotations;

namespace NCS.Infrastructure.Storage;

public sealed class LocalFileStorageOptions
{
    public const string SectionName = "Storage";

    [Required]
    public string RootPath { get; set; } = string.Empty;

    [Required]
    public string PublicUrlPrefix { get; set; } = "/uploads";
}
