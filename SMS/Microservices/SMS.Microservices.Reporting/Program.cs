using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SMS.Microservices.Reporting.Data;
using SMS.ServiceDefaults;
using SMS.Microservices.Reporting.MappingProfiles;
using FluentValidation;
using FluentValidation.AspNetCore;
using SMS.Microservices.Reporting.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ReportingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ReportingDb")));

builder.Services.AddAutoMapper(typeof(ReportingMappingProfile).Assembly);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateReportRequestValidator>();

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

