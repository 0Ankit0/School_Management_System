var builder = DistributedApplication.CreateBuilder(args);

// Add the microservices
var userService = builder.AddProject<Projects.SMS_Microservices_User>("userservice")
    .WithHttpEndpoint(port: 5001, name: "http"); // Explicitly define HTTP endpoint for API Gateway

var notificationMessagingService = builder.AddProject<Projects.SMS_Microservices_NotificationMessaging>("notificationmessagingservice")
    .WithHttpEndpoint(port: 5002, name: "http"); // Explicitly define HTTP endpoint for API Gateway

var fileManagementService = builder.AddProject<Projects.SMS_Microservices_FileManagement>("filemanagementservice")
    .WithHttpEndpoint(port: 5003, name: "http"); // Explicitly define HTTP endpoint for API Gateway

var schoolCoreService = builder.AddProject<Projects.SMS_Microservices_SchoolCore>("schoolcoreservice")
    .WithHttpEndpoint(port: 5004, name: "http"); // Explicitly define HTTP endpoint for API Gateway

// Add the API Gateway
builder.AddProject<Projects.SMS_ApiGateway>("apigateway")
    .WithReference(userService)
    .WithReference(notificationMessagingService)
    .WithReference(fileManagementService)
    .WithReference(schoolCoreService)
    .WithHttpEndpoint(port: 5000, name: "http"); // API Gateway will run on port 5000

builder.Build().Run();