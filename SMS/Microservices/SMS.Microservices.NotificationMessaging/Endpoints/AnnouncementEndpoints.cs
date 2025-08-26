using Microsoft.EntityFrameworkCore;
using SMS.Contracts.Announcements;
using SMS.Microservices.NotificationMessaging.Data;
using SMS.Microservices.NotificationMessaging.Models;
using SMS.ServiceDefaults;

namespace SMS.Microservices.NotificationMessaging.Endpoints;

public class AnnouncementEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/announcements", async (NotificationMessagingDbContext dbContext) =>
        {
            var announcements = await dbContext.Announcements.ToListAsync();
            return Results.Ok(announcements);
        });

        app.MapPost("/api/announcements", async (CreateAnnouncementRequest request, NotificationMessagingDbContext dbContext) =>
        {
            var announcement = new Announcement
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Content = request.Content,
                PublishDate = request.PublishDate,
                ExpiryDate = request.ExpiryDate,
                TargetAudience = request.TargetAudience,
                CreatedAt = DateTime.UtcNow
            };

            await dbContext.Announcements.AddAsync(announcement);
            await dbContext.SaveChangesAsync();

            return Results.Created($"/api/announcements/{announcement.Id}", null);
        });
    }
}