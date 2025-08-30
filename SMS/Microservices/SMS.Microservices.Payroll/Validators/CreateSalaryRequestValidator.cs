using FluentValidation;
using SMS.Contracts.Payroll;

namespace SMS.Microservices.Payroll.Validators;

public class CreateSalaryRequestValidator : AbstractValidator<CreateSalaryRequest>
{
    public CreateSalaryRequestValidator()
    {
        RuleFor(x => x.TeacherExternalId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.EffectiveDate).NotEmpty();
    }
}
