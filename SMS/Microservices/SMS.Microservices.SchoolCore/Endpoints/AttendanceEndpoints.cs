using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.Microservices.SchoolCore.Data;
using SMS.Microservices.SchoolCore.Models;
using SMS.Contracts.Attendances;
using AutoMapper;
using Dapper;
using System.Data;
using Npgsql;
using SMS.ServiceDefaults;

namespace SMS.Microservices.SchoolCore.Endpoints;

public class AttendanceEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/attendances", GetAttendances)
            .WithName("GetAttendances");

        app.MapGet("/api/attendances/{id}", GetAttendance)
            .WithName("GetAttendance");

        app.MapPost("/api/attendances", PostAttendance)
            .WithName("CreateAttendance");
    }

    public static async Task<IResult> GetAttendances(
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var attendances = await context.Attendances.ToListAsync();
        return Results.Ok(mapper.Map<IEnumerable<AttendanceResponse>>(attendances));
    }

    public static async Task<IResult> GetAttendance(
        Guid id,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var attendance = await context.Attendances.FirstOrDefaultAsync(a => a.ExternalId == id);

        if (attendance == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(mapper.Map<AttendanceResponse>(attendance));
    }

    public static async Task<IResult> PostAttendance(
        CreateAttendanceRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var attendance = mapper.Map<Attendance>(request);
        context.Attendances.Add(attendance);
        await context.SaveChangesAsync();

        return Results.CreatedAtRoute("GetAttendance", new { id = attendance.ExternalId }, mapper.Map<AttendanceResponse>(attendance));
    }
}
