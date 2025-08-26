// Set environment variable for Aspire dashboard OTLP endpoint
Environment.SetEnvironmentVariable("DOTNET_DASHBOARD_OTLP_ENDPOINT_URL", "http://localhost:4317");
Environment.SetEnvironmentVariable("ASPIRE_ALLOW_UNSECURED_TRANSPORT", "true");

var builder = DistributedApplication.CreateBuilder(args);

// Add the microservices using project path references
var userService = builder.AddProject("userservice", "../Microservices/SMS.Microservices.User/SMS.Microservices.User.csproj");
var notificationMessagingService = builder.AddProject("notificationmessagingservice", "../Microservices/SMS.Microservices.NotificationMessaging/SMS.Microservices.NotificationMessaging.csproj");
var fileManagementService = builder.AddProject("filemanagementservice", "../Microservices/SMS.Microservices.FileManagement/SMS.Microservices.FileManagement.csproj");
var schoolCoreService = builder.AddProject("schoolcoreservice", "../Microservices/SMS.Microservices.SchoolCore/SMS.Microservices.SchoolCore.csproj");

// Add the API Gateway
var apiGateway = builder.AddProject("apigateway", "../SMS.ApiGateway/SMS.ApiGateway.csproj")
    .WithReference(userService)
    .WithReference(notificationMessagingService)
    .WithReference(fileManagementService)
    .WithReference(schoolCoreService);

// Add the UI
builder.AddProject("sms-ui-web", "../SMS.UI/SMS.UI.Web/SMS.UI.Web.csproj")
    .WithReference(apiGateway);

builder.Build().Run();