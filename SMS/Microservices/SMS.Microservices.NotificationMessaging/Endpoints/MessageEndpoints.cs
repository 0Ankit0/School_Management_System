using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.Microservices.NotificationMessaging.Data;
using SMS.Microservices.NotificationMessaging.Models;
using SMS.Contracts.Messages;
using AutoMapper;
using Dapper;
using System.Data;
using Npgsql;
using SMS.ServiceDefaults;

namespace SMS.Microservices.NotificationMessaging.Endpoints;

public class MessageEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/messages", GetMessages)
            .WithName("GetMessages");

        app.MapGet("/api/messages/{id}", GetMessage)
            .WithName("GetMessage");

        app.MapPost("/api/messages", PostMessage)
            .WithName("CreateMessage");

        app.MapGet("/api/messages/unread/{recipientExternalId}", GetUnreadMessagesForRecipient)
            .WithName("GetUnreadMessagesForRecipient");
    }

    public static async Task<IResult> GetMessages(
        NotificationMessagingDbContext context,
        IMapper mapper)
    {
        var messages = await context.Messages.ToListAsync();
        return Results.Ok(mapper.Map<IEnumerable<MessageResponse>>(messages));
    }

    public static async Task<IResult> GetMessage(
        Guid id,
        NotificationMessagingDbContext context,
        IMapper mapper)
    {
        var message = await context.Messages.FirstOrDefaultAsync(m => m.ExternalId == id);

        if (message == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(mapper.Map<MessageResponse>(message));
    }

    public static async Task<IResult> PostMessage(
        CreateMessageRequest request,
        NotificationMessagingDbContext context,
        IMapper mapper)
    {
        var message = mapper.Map<Message>(request);
        context.Messages.Add(message);
        await context.SaveChangesAsync();

        return Results.CreatedAtRoute("GetMessage", new { id = message.ExternalId }, mapper.Map<MessageResponse>(message));
    }

    public static async Task<IResult> GetUnreadMessagesForRecipient(
        Guid recipientExternalId,
        IConfiguration configuration)
    {
        using IDbConnection dbConnection = new NpgsqlConnection(configuration.GetConnectionString("NotificationMessagingDbConnection"));
        var sql = "SELECT * FROM \"Messages\" WHERE \"RecipientExternalId\" = @RecipientExternalId AND \"IsRead\" = FALSE";
        var messages = await dbConnection.QueryAsync<Message>(sql, new { RecipientExternalId = recipientExternalId });
        // Note: Mapping to DTOs should happen here if Message is a domain model
        return Results.Ok(messages); // Assuming Message is already the DTO or will be mapped later
    }
}
