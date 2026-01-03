using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NCS.Domain.Entities;

namespace NCS.Infrastructure.Persistence.Configurations;

public sealed class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
{
    public void Configure(EntityTypeBuilder<BlogPost> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Slug).HasMaxLength(200).IsRequired();
        builder.HasIndex(x => x.Slug).IsUnique();

        builder.Property(x => x.Excerpt).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Content).IsRequired();

        builder.Property(x => x.CoverImageUrl).HasMaxLength(2000);

        var stringListConverter = new ValueConverter<List<string>, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => string.IsNullOrWhiteSpace(v)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>());

        builder.Property(x => x.Tags)
            .HasConversion(stringListConverter)
            .HasColumnType("nvarchar(max)");
    }
}
