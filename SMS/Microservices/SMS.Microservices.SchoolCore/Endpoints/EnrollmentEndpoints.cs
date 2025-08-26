using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.Microservices.SchoolCore.Data;
using SMS.Microservices.SchoolCore.Models;
using AutoMapper;
using Dapper;
using System.Data;
using Npgsql;
using SMS.ServiceDefaults;
using Microsoft.AspNetCore.Routing;
using SMS.Contracts.Enrollments;

namespace SMS.Microservices.SchoolCore.Endpoints;

public class EnrollmentEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/enrollments", GetEnrollments)
            .WithName("GetEnrollments");

        app.MapPost("/api/v1/enrollments", PostEnrollment)
            .WithName("CreateEnrollment");

        app.MapPut("/api/v1/enrollments/{id}/grade", UpdateEnrollmentGrade)
            .WithName("UpdateEnrollmentGrade");

        app.MapDelete("/api/v1/enrollments/{id}", DeleteEnrollment)
            .WithName("DeleteEnrollment");
    }

    public static async Task<IResult> GetEnrollments(
        SchoolCoreDbContext context,
        IMapper mapper,
        IConfiguration configuration)
    {
        using IDbConnection dbConnection = new NpgsqlConnection(configuration.GetConnectionString("SchoolCoreConnection"));
        var sql = "SELECT e.\"ExternalId\" as Id, s.\"ExternalId\" as StudentExternalId, s.\"FirstName\" || ' ' || s.\"LastName\" as StudentFullName, c.\"ExternalId\" as CourseExternalId, c.\"Title\" as CourseTitle, e.\"EnrollmentDate\", e.\"Grade\" FROM \"Enrollments\" e JOIN \"Students\" s ON e.\"StudentId\" = s.\"Id\" JOIN \"Courses\" c ON e.\"CourseId\" = c.\"Id\"";
        var enrollments = await dbConnection.QueryAsync<EnrollmentResponse>(sql);
        return Results.Ok(enrollments); // Assuming Enrollment is already the DTO or will be mapped later
    }

    public static async Task<IResult> PostEnrollment(
        CreateEnrollmentRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var enrollment = new Enrollment
        {
            Id = Guid.NewGuid(),
            ExternalId = Guid.NewGuid(),
            StudentId = request.StudentExternalId,
            CourseId = request.CourseExternalId,
            EnrollmentDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(), // Replace with actual user ID
            UpdatedBy = Guid.NewGuid() // Replace with actual user ID
        };

        context.Enrollments.Add(enrollment);
        await context.SaveChangesAsync();

        return Results.Created($"/api/v1/enrollments/{enrollment.ExternalId}", mapper.Map<EnrollmentResponse>(enrollment));
    }

    public static async Task<IResult> UpdateEnrollmentGrade(
        Guid id,
        UpdateGradeRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var enrollment = await context.Enrollments.FirstOrDefaultAsync(e => e.ExternalId == id);
        if (enrollment == null)
        {
            return Results.NotFound();
        }

        enrollment.Grade = request.Grade;
        enrollment.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteEnrollment(
        Guid id,
        SchoolCoreDbContext context)
    {
        var enrollment = await context.Enrollments.FirstOrDefaultAsync(e => e.ExternalId == id);
        if (enrollment == null)
        {
            return Results.NotFound();
        }

        context.Enrollments.Remove(enrollment);
        await context.SaveChangesAsync();
        return Results.NoContent();
    }
}
