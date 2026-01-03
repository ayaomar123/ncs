using MediatR;
using NCS.Application.Features.Donations.Dtos;
using NCS.Application.Interfaces.Repositories;
using NCS.Domain.Entities;
using NCS.Domain.Enums;

namespace NCS.Application.Features.Donations.Commands;

public sealed record CreateDonationRequestCommand(
    decimal Amount,
    string Currency,
    DonationType Type,
    DonationCategory Category,
    Guid? AppealId,
    string DonorName,
    string DonorEmail) : IRequest<DonationRequestCreatedDto>;

public sealed class CreateDonationRequestCommandHandler(IDonationRepository repository) : IRequestHandler<CreateDonationRequestCommand, DonationRequestCreatedDto>
{
    public async Task<DonationRequestCreatedDto> Handle(CreateDonationRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = new DonationRequest
        {
            Id = Guid.NewGuid(),
            Amount = request.Amount,
            Currency = string.IsNullOrWhiteSpace(request.Currency) ? "GBP" : request.Currency.Trim().ToUpperInvariant(),
            Type = request.Type,
            Category = request.Category,
            AppealId = request.AppealId,
            DonorName = request.DonorName.Trim(),
            DonorEmail = request.DonorEmail.Trim(),
            Status = DonationStatus.Pending,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await repository.AddAsync(entity, cancellationToken);

        return new DonationRequestCreatedDto(entity.Id, "/donate/success");
    }
}
