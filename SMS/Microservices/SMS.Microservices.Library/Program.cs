using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SMS.Microservices.Library.Data;
using SMS.ServiceDefaults;
using SMS.Microservices.Library.MappingProfiles;
using FluentValidation;
using FluentValidation.AspNetCore;
using SMS.Microservices.Library.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("LibraryDb")));

builder.Services.AddAutoMapper(typeof(LibraryMappingProfile).Assembly);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookRequestValidator>();

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

