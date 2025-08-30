using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SMS.Microservices.Payroll.Data;
using SMS.ServiceDefaults;
using SMS.Microservices.Payroll.MappingProfiles;
using FluentValidation;
using FluentValidation.AspNetCore;
using SMS.Microservices.Payroll.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PayrollDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PayrollDb")));

builder.Services.AddAutoMapper(typeof(PayrollMappingProfile).Assembly);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateSalaryRequestValidator>();

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

