using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SMS.Microservices.NotificationMessaging.Data;
using SMS.Microservices.NotificationMessaging.Endpoints;
using SMS.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder.Services.AddDbContext<NotificationMessagingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("NotificationMessagingConnection")));

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["MessageBroker:Host"], "/", h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"]);
            h.Password(builder.Configuration["MessageBroker:Password"]);
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient<IEndpoint, AnnouncementEndpoints>();
builder.Services.AddTransient<IEndpoint, MessageEndpoints>();
builder.Services.AddTransient<IEndpoint, NotificationEndpoints>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapDefaultEndpoints();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<NotificationMessagingDbContext>();
    context.Database.Migrate();
}

app.Run();
