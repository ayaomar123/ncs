using NCS.Application.Interfaces.Repositories;
using NCS.Domain.Entities;
using NCS.Infrastructure.Persistence;

namespace NCS.Infrastructure.Repositories;

public sealed class DonationRepository(NcsDbContext db) : IDonationRepository
{
    public async Task AddAsync(DonationRequest donationRequest, CancellationToken cancellationToken)
    {
        db.DonationRequests.Add(donationRequest);
        await db.SaveChangesAsync(cancellationToken);
    }
}
