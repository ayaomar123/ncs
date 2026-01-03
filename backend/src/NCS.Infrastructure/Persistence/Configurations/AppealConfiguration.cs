using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NCS.Domain.Entities;

namespace NCS.Infrastructure.Persistence.Configurations;

public sealed class AppealConfiguration : IEntityTypeConfiguration<Appeal>
{
    public void Configure(EntityTypeBuilder<Appeal> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Slug).HasMaxLength(200).IsRequired();
        builder.HasIndex(x => x.Slug).IsUnique();

        builder.Property(x => x.Summary).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Description).IsRequired();

        builder.Property(x => x.CountryTag).HasMaxLength(100);
        builder.Property(x => x.CoverImageUrl).HasMaxLength(2000);

        builder.Property(x => x.TargetAmount).HasPrecision(18, 2);
        builder.Property(x => x.RaisedAmount).HasPrecision(18, 2);

        var stringListConverter = new ValueConverter<List<string>, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => string.IsNullOrWhiteSpace(v)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>());

        builder.Property(x => x.GalleryUrls)
            .HasConversion(stringListConverter)
            .HasColumnType("nvarchar(max)");
    }
}
