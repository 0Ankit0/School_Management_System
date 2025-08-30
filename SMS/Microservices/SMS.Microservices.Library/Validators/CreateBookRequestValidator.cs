using FluentValidation;
using SMS.Contracts.Library;

namespace SMS.Microservices.Library.Validators;

public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
{
    public CreateBookRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Author).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ISBN).NotEmpty().MaximumLength(20);
        RuleFor(x => x.PublishedDate).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
    }
}
