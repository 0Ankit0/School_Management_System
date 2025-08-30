using FluentValidation;
using SMS.Contracts.Payroll;

namespace SMS.Microservices.Payroll.Validators;

public class UpdateBonusRequestValidator : AbstractValidator<UpdateBonusRequest>
{
    public UpdateBonusRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.TeacherExternalId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.BonusDate).NotEmpty();
    }
}
