using FluentValidation;
using NCS.Application.Features.Appeals.Commands;

namespace NCS.Application.Features.Appeals.Validators;

public sealed class UpdateAppealCommandValidator : AbstractValidator<UpdateAppealCommand>
{
    public UpdateAppealCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Slug).MaximumLength(200);
        RuleFor(x => x.Summary).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.CountryTag).MaximumLength(100);
        RuleFor(x => x.TargetAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.RaisedAmount).GreaterThanOrEqualTo(0);
        RuleForEach(x => x.GalleryUrls).MaximumLength(2000);
        RuleFor(x => x.CoverImageUrl).MaximumLength(2000);
    }
}
