namespace NCS.Application.Features.Donations.Dtos;

public sealed record DonationRequestCreatedDto(Guid DonationRequestId, string RedirectUrl);
