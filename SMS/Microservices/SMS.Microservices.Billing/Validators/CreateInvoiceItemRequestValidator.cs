using FluentValidation;
using SMS.Contracts.Billing;

namespace SMS.Microservices.Billing.Validators;

public class CreateInvoiceItemRequestValidator : AbstractValidator<CreateInvoiceItemRequest>
{
    public CreateInvoiceItemRequestValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}
