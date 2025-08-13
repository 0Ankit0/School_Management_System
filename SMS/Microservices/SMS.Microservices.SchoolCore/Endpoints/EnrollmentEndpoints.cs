using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.Microservices.SchoolCore.Data;
using SMS.Microservices.SchoolCore.Models;
using AutoMapper;
using Dapper;
using System.Data;
using Npgsql;
using SMS.ServiceDefaults;

namespace SMS.Microservices.SchoolCore.Endpoints;

public class EnrollmentEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/enrollments", GetEnrollments)
            .WithName("GetEnrollments");

        app.MapGet("/api/enrollments/{id}", GetEnrollment)
            .WithName("GetEnrollment");

        app.MapPost("/api/enrollments", PostEnrollment)
            .WithName("CreateEnrollment");
    }

    public static async Task<IResult> GetEnrollments(
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var enrollments = await context.Enrollments.ToListAsync();
        return Results.Ok(enrollments); // Assuming Enrollment is already the DTO or will be mapped later
    }

    public static async Task<IResult> GetEnrollment(
        Guid id,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var enrollment = await context.Enrollments.FirstOrDefaultAsync(e => e.ExternalId == id);

        if (enrollment == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(enrollment); // Assuming Enrollment is already the DTO or will be mapped later
    }

    public static async Task<IResult> PostEnrollment(
        Enrollment enrollment,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        context.Enrollments.Add(enrollment);
        await context.SaveChangesAsync();

        return Results.CreatedAtRoute("GetEnrollment", new { id = enrollment.ExternalId }, enrollment); // Assuming Enrollment is already the DTO or will be mapped later
    }
}
