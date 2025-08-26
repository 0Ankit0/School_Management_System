using Microsoft.EntityFrameworkCore;
using SMS.Contracts.Notifications;
using SMS.Microservices.NotificationMessaging.Data;
using SMS.Microservices.NotificationMessaging.Models;
using SMS.ServiceDefaults;
using Microsoft.AspNetCore.Routing;
using AutoMapper;
using Dapper;
using System.Data;
using Npgsql;
using Microsoft.AspNetCore.Mvc;

namespace SMS.Microservices.NotificationMessaging.Endpoints;

public class NotificationEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/notifications", GetNotifications)
            .WithName("GetNotifications");

        app.MapPost("/api/v1/notifications", PostNotification)
            .WithName("CreateNotification");
    }

    public static async Task<IResult> GetNotifications(
        NotificationMessagingDbContext context,
        IMapper mapper,
        IConfiguration configuration,
        [FromQuery] Guid? userId = null)
    {
        using IDbConnection dbConnection = new NpgsqlConnection(configuration.GetConnectionString("NotificationMessagingConnection"));
        var sql = "SELECT \"ExternalId\" as Id, \"UserId\" as UserExternalId, \"Content\", \"Type\", \"IsRead\", \"CreatedAt\" FROM \"Notifications\" WHERE 1=1";
        
        if (userId.HasValue)
        {
            sql += " AND \"UserId\" = @UserId";
        }

        var notifications = await dbConnection.QueryAsync<NotificationResponse>(sql, new { UserId = userId });
        return Results.Ok(notifications);
    }

    public static async Task<IResult> PostNotification(
        CreateNotificationRequest request,
        NotificationMessagingDbContext context,
        IMapper mapper)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            ExternalId = Guid.NewGuid(),
            UserId = request.UserExternalId,
            Content = request.Content,
            Type = request.Type,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        context.Notifications.Add(notification);
        await context.SaveChangesAsync();

        return Results.Created($"/api/v1/notifications/{notification.ExternalId}", mapper.Map<NotificationResponse>(notification));
    }
}
