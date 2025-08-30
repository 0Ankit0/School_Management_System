using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMS.Contracts.Billing;
using SMS.Microservices.Billing.Data;
using SMS.Microservices.Billing.Models;
using SMS.ServiceDefaults;

namespace SMS.Microservices.Billing.Endpoints;

public class BillingEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/billing/invoices", async (BillingDbContext db) =>
        {
            return await db.Invoices.ToListAsync();
        });

        app.MapGet("/api/billing/invoices/{id}", async (Guid id, BillingDbContext db, IMapper mapper) =>
        {
            var invoice = await db.Invoices.Include(i => i.Items).FirstOrDefaultAsync(i => i.Id == id);
            return invoice == null ? Results.NotFound() : Results.Ok(mapper.Map<InvoiceResponse>(invoice));
        });

        app.MapPost("/api/billing/invoices", async (CreateInvoiceRequest request, BillingDbContext db, IMapper mapper) =>
        {
            var invoice = mapper.Map<Invoice>(request);
            invoice.Id = Guid.NewGuid();
            invoice.CreatedAt = DateTime.UtcNow;
            invoice.UpdatedAt = DateTime.UtcNow;
            // TODO: Get CreatedBy and UpdatedBy from authenticated user
            invoice.CreatedBy = Guid.Empty;
            invoice.UpdatedBy = Guid.Empty;

            foreach (var item in invoice.Items)
            {
                item.Id = Guid.NewGuid();
                item.InvoiceId = invoice.Id;
                item.CreatedAt = DateTime.UtcNow;
                item.UpdatedAt = DateTime.UtcNow;
                // TODO: Get CreatedBy and UpdatedBy from authenticated user
                item.CreatedBy = Guid.Empty;
                item.UpdatedBy = Guid.Empty;
            }

            db.Invoices.Add(invoice);
            await db.SaveChangesAsync();

            return Results.Created($"/api/billing/invoices/{invoice.Id}", mapper.Map<InvoiceResponse>(invoice));
        });

        app.MapPut("/api/billing/invoices/{id}", async (Guid id, UpdateInvoiceRequest request, BillingDbContext db, IMapper mapper) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest();
            }

            var invoice = await db.Invoices.Include(i => i.Items).FirstOrDefaultAsync(i => i.Id == id);
            if (invoice == null)
            {
                return Results.NotFound();
            }

            mapper.Map(request, invoice);
            invoice.UpdatedAt = DateTime.UtcNow;
            // TODO: Get UpdatedBy from authenticated user
            invoice.UpdatedBy = Guid.Empty;

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapDelete("/api/billing/invoices/{id}", async (Guid id, BillingDbContext db) =>
        {
            var invoice = await db.Invoices.FirstOrDefaultAsync(i => i.Id == id);
            if (invoice == null)
            {
                return Results.NotFound();
            }

            db.Invoices.Remove(invoice);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        // Invoice Items Endpoints
        app.MapGet("/api/billing/invoices/{invoiceId}/items/{id}", async (Guid invoiceId, Guid id, BillingDbContext db, IMapper mapper) =>
        {
            var invoiceItem = await db.InvoiceItems.FirstOrDefaultAsync(item => item.InvoiceId == invoiceId && item.Id == id);
            return invoiceItem == null ? Results.NotFound() : Results.Ok(mapper.Map<InvoiceItemResponse>(invoiceItem));
        });

        app.MapPost("/api/billing/invoices/{invoiceId}/items", async (Guid invoiceId, CreateInvoiceItemRequest request, BillingDbContext db, IMapper mapper) =>
        {
            var invoice = await db.Invoices.FirstOrDefaultAsync(i => i.Id == invoiceId);
            if (invoice == null)
            {
                return Results.NotFound("Invoice not found.");
            }

            var invoiceItem = mapper.Map<InvoiceItem>(request);
            invoiceItem.Id = Guid.NewGuid();
            invoiceItem.InvoiceId = invoiceId;
            invoiceItem.CreatedAt = DateTime.UtcNow;
            invoiceItem.UpdatedAt = DateTime.UtcNow;
            // TODO: Get CreatedBy and UpdatedBy from authenticated user
            invoiceItem.CreatedBy = Guid.Empty;
            invoiceItem.UpdatedBy = Guid.Empty;

            db.InvoiceItems.Add(invoiceItem);
            await db.SaveChangesAsync();

            return Results.Created($"/api/billing/invoices/{invoiceId}/items/{invoiceItem.Id}", mapper.Map<InvoiceItemResponse>(invoiceItem));
        });

        app.MapPut("/api/billing/invoices/{invoiceId}/items/{id}", async (Guid invoiceId, Guid id, UpdateInvoiceItemRequest request, BillingDbContext db, IMapper mapper) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest();
            }

            var invoiceItem = await db.InvoiceItems.FirstOrDefaultAsync(item => item.InvoiceId == invoiceId && item.Id == id);
            if (invoiceItem == null)
            {
                return Results.NotFound();
            }

            mapper.Map(request, invoiceItem);
            invoiceItem.UpdatedAt = DateTime.UtcNow;
            // TODO: Get UpdatedBy from authenticated user
            invoiceItem.UpdatedBy = Guid.Empty;

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapDelete("/api/billing/invoices/{invoiceId}/items/{id}", async (Guid invoiceId, Guid id, BillingDbContext db) =>
        {
            var invoiceItem = await db.InvoiceItems.FirstOrDefaultAsync(item => item.InvoiceId == invoiceId && item.Id == id);
            if (invoiceItem == null)
            {
                return Results.NotFound();
            }

            db.InvoiceItems.Remove(invoiceItem);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        // Payments Endpoints
        app.MapGet("/api/billing/payments/{id}", async (Guid id, BillingDbContext db, IMapper mapper) =>
        {
            var payment = await db.Payments.FirstOrDefaultAsync(p => p.Id == id);
            return payment == null ? Results.NotFound() : Results.Ok(mapper.Map<PaymentResponse>(payment));
        });

        app.MapPost("/api/billing/payments", async (CreatePaymentRequest request, BillingDbContext db, IMapper mapper) =>
        {
            var payment = mapper.Map<Payment>(request);
            payment.Id = Guid.NewGuid();
            payment.CreatedAt = DateTime.UtcNow;
            payment.UpdatedAt = DateTime.UtcNow;
            // TODO: Get CreatedBy and UpdatedBy from authenticated user
            payment.CreatedBy = Guid.Empty;
            payment.UpdatedBy = Guid.Empty;

            db.Payments.Add(payment);
            await db.SaveChangesAsync();

            return Results.Created($"/api/billing/payments/{payment.Id}", mapper.Map<PaymentResponse>(payment));
        });

        app.MapPut("/api/billing/payments/{id}", async (Guid id, UpdatePaymentRequest request, BillingDbContext db, IMapper mapper) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest();
            }

            var payment = await db.Payments.FirstOrDefaultAsync(p => p.Id == id);
            if (payment == null)
            {
                return Results.NotFound();
            }

            mapper.Map(request, payment);
            payment.UpdatedAt = DateTime.UtcNow;
            // TODO: Get UpdatedBy from authenticated user
            payment.UpdatedBy = Guid.Empty;

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapDelete("/api/billing/payments/{id}", async (Guid id, BillingDbContext db) =>
        {
            var payment = await db.Payments.FirstOrDefaultAsync(p => p.Id == id);
            if (payment == null)
            {
                return Results.NotFound();
            }

            db.Payments.Remove(payment);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}

