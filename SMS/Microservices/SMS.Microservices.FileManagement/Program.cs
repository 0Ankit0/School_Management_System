using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SMS.Microservices.FileManagement.Data;
using SMS.Microservices.FileManagement.Endpoints;
using SMS.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder.Services.AddDbContext<FileManagementDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("FileManagementConnection")));

builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration.GetConnectionString("AzureStorage")));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient<IEndpoint, FileEndpoints>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapDefaultEndpoints();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<FileManagementDbContext>();
    context.Database.Migrate();
}

app.Run();
