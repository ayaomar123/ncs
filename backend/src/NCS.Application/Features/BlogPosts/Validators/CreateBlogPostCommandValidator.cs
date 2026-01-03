using FluentValidation;
using NCS.Application.Features.BlogPosts.Commands;

namespace NCS.Application.Features.BlogPosts.Validators;

public sealed class CreateBlogPostCommandValidator : AbstractValidator<CreateBlogPostCommand>
{
    public CreateBlogPostCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Slug).MaximumLength(200);
        RuleFor(x => x.Excerpt).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.CoverImageUrl).MaximumLength(2000);
        RuleForEach(x => x.Tags).MaximumLength(50);
    }
}
