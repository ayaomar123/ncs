using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCS.Domain.Entities;

namespace NCS.Infrastructure.Persistence.Configurations;

public sealed class DonationRequestConfiguration : IEntityTypeConfiguration<DonationRequest>
{
    public void Configure(EntityTypeBuilder<DonationRequest> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Currency).HasMaxLength(10).IsRequired();
        builder.Property(x => x.Amount).HasPrecision(18, 2);

        builder.Property(x => x.DonorName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.DonorEmail).HasMaxLength(200).IsRequired();

        builder.HasOne<Appeal>()
            .WithMany()
            .HasForeignKey(x => x.AppealId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
