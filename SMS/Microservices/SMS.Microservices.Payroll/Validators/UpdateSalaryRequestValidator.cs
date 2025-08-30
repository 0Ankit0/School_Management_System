using FluentValidation;
using SMS.Contracts.Payroll;

namespace SMS.Microservices.Payroll.Validators;

public class UpdateSalaryRequestValidator : AbstractValidator<UpdateSalaryRequest>
{
    public UpdateSalaryRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.TeacherExternalId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.EffectiveDate).NotEmpty();
    }
}
