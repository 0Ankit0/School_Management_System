using SMS.Microservices.NotificationMessaging.MappingProfiles;
using Polly;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using SMS.Microservices.NotificationMessaging.Data;
using SMS.ServiceDefaults;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddControllers(); // Removed for Minimal APIs
// builder.Services.AddEndpointsApiExplorer(); // Configured differently for Minimal APIs
// builder.Services.AddSwaggerGen(); // Configured differently for Minimal APIs

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(NotificationMessagingMappingProfile).Assembly);

// Configure HttpClient with Polly for resilience
builder.Services.AddHttpClient("ResilientClient")
    .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.WaitAndRetryAsync(new[]
    {
        TimeSpan.FromSeconds(1),
        TimeSpan.FromSeconds(5),
        TimeSpan.FromSeconds(10)
    }));

// Configure MassTransit with RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost"); // Replace with your RabbitMQ host
        cfg.ConfigureEndpoints(context);
    });
});

// Configure DbContext
builder.Services.AddDbContext<NotificationMessagingDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("NotificationMessagingDbConnection"));
});

// Add Aspire Service Defaults
builder.AddServiceDefaults();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Swagger will need to be configured for Minimal APIs
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.UseAuthorization(); // Handled by Minimal API endpoint configuration

// Dynamically register Minimal API endpoints
var assemblies = AppDomain.CurrentDomain.GetAssemblies();
foreach (var assembly in assemblies)
{
    var endpointTypes = assembly.GetTypes()
        .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

    foreach (var endpointType in endpointTypes)
    {
        var endpoint = (IEndpoint)Activator.CreateInstance(endpointType);
        endpoint.MapEndpoints(app);
    }
}

// app.MapControllers(); // Removed for Minimal APIs

// Use Aspire Service Defaults
app.UseServiceDefaults();

app.Run();