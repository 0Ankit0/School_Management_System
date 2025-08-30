using FluentValidation;
using System;
using System.Collections.Generic;

namespace SMS.Contracts.Billing;

public class CreateInvoiceRequest
{
    public Guid StudentExternalId { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public List<CreateInvoiceItemRequest> Items { get; set; }
}

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

public class CreateInvoiceItemRequest
{
    public string Description { get; set; }
    public decimal Amount { get; set; }
}

public class CreateInvoiceItemRequestValidator : AbstractValidator<CreateInvoiceItemRequest>
{
    public CreateInvoiceItemRequestValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}

public class CreatePaymentRequest
{
    public Guid InvoiceExternalId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
}

public class CreatePaymentRequestValidator : AbstractValidator<CreatePaymentRequest>
{
    public CreatePaymentRequestValidator()
    {
        RuleFor(x => x.InvoiceExternalId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.PaymentDate).NotEmpty();
    }
}

public class InvoiceResponse
{
    public Guid Id { get; set; } // Maps to ExternalId
    public Guid StudentExternalId { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public List<InvoiceItemResponse> Items { get; set; }
}

public class InvoiceItemResponse
{
    public Guid Id { get; set; } // Maps to ExternalId
    public string Description { get; set; }
    public decimal Amount { get; set; }
}

public class PaymentResponse
{
    public Guid Id { get; set; } // Maps to ExternalId
    public Guid InvoiceExternalId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
}

public class UpdateInvoiceRequest
{
    public Guid Id { get; set; }
    public Guid StudentExternalId { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
}

public class UpdateInvoiceRequestValidator : AbstractValidator<UpdateInvoiceRequest>
{
    public UpdateInvoiceRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.StudentExternalId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.DueDate).NotEmpty();
    }
}

public class UpdateInvoiceItemRequest
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
}

public class UpdateInvoiceItemRequestValidator : AbstractValidator<UpdateInvoiceItemRequest>
{
    public UpdateInvoiceItemRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}

public class UpdatePaymentRequest
{
    public Guid Id { get; set; }
    public Guid InvoiceExternalId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
}

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
