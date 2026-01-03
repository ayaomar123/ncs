using NCS.Domain.Entities;

namespace NCS.Application.Interfaces.Repositories;

public interface IDonationRepository
{
    Task AddAsync(DonationRequest donationRequest, CancellationToken cancellationToken);
}
