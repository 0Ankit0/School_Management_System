using FluentValidation;
using SMS.Contracts.Billing;

namespace SMS.Microservices.Billing.Validators;

public class CreateInvoiceRequestValidator : AbstractValidator<CreateInvoiceRequest>
{
    public CreateInvoiceRequestValidator()
    {
        RuleFor(x => x.StudentExternalId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.DueDate).NotEmpty();
        RuleForEach(x => x.Items).SetValidator(new CreateInvoiceItemRequestValidator());
    }
}
