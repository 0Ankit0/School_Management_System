using FluentValidation;
using SMS.Contracts.Reporting;

namespace SMS.Microservices.Reporting.Validators;

public class CreateReportRequestValidator : AbstractValidator<CreateReportRequest>
{
    public CreateReportRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.Query).NotEmpty();
    }
}
