using FluentValidation;
using NCS.Application.Features.Contact.Commands;

namespace NCS.Application.Features.Contact.Validators;

public sealed class CreateContactMessageCommandValidator : AbstractValidator<CreateContactMessageCommand>
{
    public CreateContactMessageCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.Subject).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Message).NotEmpty().MaximumLength(2000);
    }
}
