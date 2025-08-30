using FluentValidation;
using SMS.Contracts.Billing;

namespace SMS.Microservices.Billing.Validators;

public class UpdateInvoiceItemRequestValidator : AbstractValidator<UpdateInvoiceItemRequest>
{
    public UpdateInvoiceItemRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}
