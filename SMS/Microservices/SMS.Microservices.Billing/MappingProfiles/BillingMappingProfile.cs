using AutoMapper;
using SMS.Contracts.Billing;
using SMS.Microservices.Billing.Models;

namespace SMS.Microservices.Billing.MappingProfiles;

public class BillingMappingProfile : Profile
{
    public BillingMappingProfile()
    {
        CreateMap<Invoice, InvoiceResponse>();
        CreateMap<InvoiceItem, InvoiceItemResponse>();
        CreateMap<Payment, PaymentResponse>();
        CreateMap<CreateInvoiceRequest, Invoice>();
        CreateMap<CreateInvoiceItemRequest, InvoiceItem>();
        CreateMap<UpdateInvoiceRequest, Invoice>();
        CreateMap<UpdateInvoiceItemRequest, InvoiceItem>();
        CreateMap<UpdatePaymentRequest, Payment>();
    }
}
