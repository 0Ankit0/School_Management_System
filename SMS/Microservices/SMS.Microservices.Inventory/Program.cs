using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SMS.Microservices.Inventory.Data;
using SMS.ServiceDefaults;
using SMS.Microservices.Inventory.MappingProfiles;
using FluentValidation;
using FluentValidation.AspNetCore;
using SMS.Microservices.Inventory.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("InventoryDb")));

builder.Services.AddAutoMapper(typeof(InventoryMappingProfile).Assembly);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateInventoryItemRequestValidator>();

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

