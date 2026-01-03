using FluentValidation;
using NCS.Application.Features.Donations.Commands;

namespace NCS.Application.Features.Donations.Validators;

public sealed class CreateDonationRequestCommandValidator : AbstractValidator<CreateDonationRequestCommand>
{
    public CreateDonationRequestCommandValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Currency).NotEmpty().MaximumLength(10);
        RuleFor(x => x.DonorName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DonorEmail).NotEmpty().EmailAddress().MaximumLength(200);
    }
}
