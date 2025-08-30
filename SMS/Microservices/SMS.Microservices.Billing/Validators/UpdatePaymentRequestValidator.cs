using FluentValidation;
using SMS.Contracts.Billing;

namespace SMS.Microservices.Billing.Validators;

public class UpdatePaymentRequestValidator : AbstractValidator<UpdatePaymentRequest>
{
    public UpdatePaymentRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.InvoiceExternalId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.PaymentDate).NotEmpty();
    }
}
