using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.Microservices.NotificationMessaging.Data;
using SMS.Microservices.NotificationMessaging.Models;
using SMS.Contracts.Announcements;
using AutoMapper;
using Dapper;
using System.Data;
using Npgsql;
using SMS.ServiceDefaults;

namespace SMS.Microservices.NotificationMessaging.Endpoints;

public class AnnouncementEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/announcements", GetAnnouncements)
            .WithName("GetAnnouncements");

        app.MapGet("/api/announcements/{id}", GetAnnouncement)
            .WithName("GetAnnouncement");

        app.MapPost("/api/announcements", PostAnnouncement)
            .WithName("CreateAnnouncement");

        app.MapGet("/api/announcements/active/{targetAudience}", GetActiveAnnouncements)
            .WithName("GetActiveAnnouncements");
    }

    public static async Task<IResult> GetAnnouncements(
        NotificationMessagingDbContext context,
        IMapper mapper)
    {
        var announcements = await context.Announcements.ToListAsync();
        return Results.Ok(mapper.Map<IEnumerable<AnnouncementResponse>>(announcements));
    }

    public static async Task<IResult> GetAnnouncement(
        Guid id,
        NotificationMessagingDbContext context,
        IMapper mapper)
    {
        var announcement = await context.Announcements.FirstOrDefaultAsync(a => a.ExternalId == id);

        if (announcement == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(mapper.Map<AnnouncementResponse>(announcement));
    }

    public static async Task<IResult> PostAnnouncement(
        CreateAnnouncementRequest request,
        NotificationMessagingDbContext context,
        IMapper mapper)
    {
        var announcement = mapper.Map<Announcement>(request);
        context.Announcements.Add(announcement);
        await context.SaveChangesAsync();

        return Results.CreatedAtRoute("GetAnnouncement", new { id = announcement.ExternalId }, mapper.Map<AnnouncementResponse>(announcement));
    }

    public static async Task<IResult> GetActiveAnnouncements(
        string targetAudience,
        IConfiguration configuration)
    {
        using IDbConnection dbConnection = new NpgsqlConnection(configuration.GetConnectionString("NotificationMessagingDbConnection"));
        var sql = "SELECT * FROM \"Announcements\" WHERE \"TargetAudience\" = @TargetAudience AND \"PublishDate\" <= NOW() AND (\"ExpiryDate\" IS NULL OR \"ExpiryDate\" >= NOW())";
        var announcements = await dbConnection.QueryAsync<Announcement>(sql, new { TargetAudience = targetAudience });
        // Note: Mapping to DTOs should happen here if Announcement is a domain model
        return Results.Ok(announcements); // Assuming Announcement is already the DTO or will be mapped later
    }
}
