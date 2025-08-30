using FluentValidation;
using SMS.Contracts.Reporting;

namespace SMS.Microservices.Reporting.Validators;

public class UpdateReportRequestValidator : AbstractValidator<UpdateReportRequest>
{
    public UpdateReportRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.Query).NotEmpty();
    }
}
