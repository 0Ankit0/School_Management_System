using Microsoft.EntityFrameworkCore;
using SMS.Contracts.Messages;
using SMS.Microservices.NotificationMessaging.Data;
using SMS.Microservices.NotificationMessaging.Models;
using SMS.ServiceDefaults;

namespace SMS.Microservices.NotificationMessaging.Endpoints;

public class MessageEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/messages", async (NotificationMessagingDbContext dbContext) =>
        {
            var messages = await dbContext.Messages.ToListAsync();
            return Results.Ok(messages);
        });

        app.MapPost("/api/messages", async (CreateMessageRequest request, NotificationMessagingDbContext dbContext) =>
        {
            var message = new Message
            {
                Id = Guid.NewGuid(),
                SenderId = Guid.NewGuid(), // Replace with actual sender ID
                RecipientId = request.RecipientExternalId,
                Content = request.Content,
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            await dbContext.Messages.AddAsync(message);
            await dbContext.SaveChangesAsync();

            return Results.Created($"/api/messages/{message.Id}", null);
        });
    }
}