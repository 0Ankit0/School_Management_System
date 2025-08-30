using FluentValidation;
using SMS.Contracts.Library;

namespace SMS.Microservices.Library.Validators;

public class UpdateBookLoanRequestValidator : AbstractValidator<UpdateBookLoanRequest>
{
    public UpdateBookLoanRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.BookExternalId).NotEmpty();
        RuleFor(x => x.StudentExternalId).NotEmpty();
        RuleFor(x => x.LoanDate).NotEmpty();
        RuleFor(x => x.DueDate).NotEmpty();
    }
}
