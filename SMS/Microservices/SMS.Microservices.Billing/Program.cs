using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SMS.Microservices.Billing.Data;
using SMS.ServiceDefaults;
using SMS.Microservices.Billing.MappingProfiles;
using SMS.Microservices.Billing.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<BillingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BillingDb")));

builder.Services.AddAutoMapper(typeof(BillingMappingProfile).Assembly);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateInvoiceRequestValidator>();

builder.AddEndpoints();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();

