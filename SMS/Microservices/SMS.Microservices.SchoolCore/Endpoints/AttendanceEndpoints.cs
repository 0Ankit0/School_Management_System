using Microsoft.EntityFrameworkCore;
using SMS.Contracts.Attendances;
using SMS.Microservices.SchoolCore.Data;
using SMS.Microservices.SchoolCore.Models;
using SMS.ServiceDefaults;
using Microsoft.AspNetCore.Routing;
using AutoMapper;
using Dapper;
using System.Data;
using Npgsql;

namespace SMS.Microservices.SchoolCore.Endpoints;

public class AttendanceEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/attendance", async (SchoolCoreDbContext dbContext) =>
        {
            var attendances = await dbContext.Attendances.ToListAsync();
            return Results.Ok(attendances);
        });

        app.MapPost("/api/attendance", async (CreateAttendanceRequest request, SchoolCoreDbContext dbContext) =>
        {
            var attendance = new Attendance
            {
                Id = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                StudentId = request.StudentExternalId,
                Date = request.Date,
                Status = request.Status,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = Guid.NewGuid(), // Replace with actual user ID
                UpdatedBy = Guid.NewGuid() // Replace with actual user ID
            };

            await dbContext.Attendances.AddAsync(attendance);
            await dbContext.SaveChangesAsync();

            return Results.Created($"/api/attendance/{attendance.Id}", null);
        });
    }
}
