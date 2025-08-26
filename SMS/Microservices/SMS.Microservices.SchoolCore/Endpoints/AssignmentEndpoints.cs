using Microsoft.EntityFrameworkCore;
using SMS.Contracts.Assignments;
using SMS.Microservices.SchoolCore.Data;
using SMS.Microservices.SchoolCore.Models;
using SMS.ServiceDefaults;
using Microsoft.AspNetCore.Routing;
using AutoMapper;
using Dapper;
using System.Data;
using Npgsql;
using Microsoft.AspNetCore.Mvc;
using SMS.Contracts.AssignmentSubmissions;

namespace SMS.Microservices.SchoolCore.Endpoints;

public class AssignmentEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/assignments", GetAssignments)
            .WithName("GetAssignments");

        app.MapPost("/api/v1/assignments", PostAssignment)
            .WithName("CreateAssignment");

        app.MapPut("/api/v1/assignments/{id}", UpdateAssignment)
            .WithName("UpdateAssignment");

        app.MapDelete("/api/v1/assignments/{id}", DeleteAssignment)
            .WithName("DeleteAssignment");

        app.MapPost("/api/v1/assignments/{id}/submit", SubmitAssignment)
            .WithName("SubmitAssignment");
    }

    public static async Task<IResult> GetAssignments(
        SchoolCoreDbContext context,
        IMapper mapper,
        IConfiguration configuration,
        [FromQuery] Guid? courseId = null,
        [FromQuery] Guid? studentId = null)
    {
        using IDbConnection dbConnection = new NpgsqlConnection(configuration.GetConnectionString("SchoolCoreConnection"));
        var sql = "SELECT a.\"ExternalId\" as Id, c.\"ExternalId\" as CourseExternalId, c.\"Title\" as CourseTitle, a.\"Title\", a.\"Description\", a.\"DueDate\" FROM \"Assignments\" a JOIN \"Courses\" c ON a.\"CourseId\" = c.\"Id\" WHERE 1=1";
        
        if (courseId.HasValue)
        {
            sql += " AND c.\"ExternalId\" = @CourseId";
        }
        // Add logic for studentId if assignments are linked to students directly or via enrollments

        var assignments = await dbConnection.QueryAsync<AssignmentResponse>(sql, new { CourseId = courseId });
        return Results.Ok(assignments);
    }

    public static async Task<IResult> PostAssignment(
        CreateAssignmentRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var course = await context.Courses.FirstOrDefaultAsync(c => c.ExternalId == request.CourseExternalId);
        if (course == null)
        {
            return Results.BadRequest("Course not found.");
        }

        var assignment = new Assignment
        {
            Id = Guid.NewGuid(),
            ExternalId = Guid.NewGuid(),
            CourseId = course.Id,
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(), // Replace with actual user ID
            UpdatedBy = Guid.NewGuid() // Replace with actual user ID
        };

        context.Assignments.Add(assignment);
        await context.SaveChangesAsync();

        return Results.Created($"/api/v1/assignments/{assignment.ExternalId}", mapper.Map<AssignmentResponse>(assignment));
    }

    public static async Task<IResult> UpdateAssignment(
        Guid id,
        UpdateAssignmentRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var assignment = await context.Assignments.FirstOrDefaultAsync(a => a.ExternalId == id);
        if (assignment == null)
        {
            return Results.NotFound();
        }

        if (!string.IsNullOrEmpty(request.Title))
        {
            assignment.Title = request.Title;
        }
        if (!string.IsNullOrEmpty(request.Description))
        {
            assignment.Description = request.Description;
        }
        assignment.DueDate = request.DueDate;
        assignment.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteAssignment(
        Guid id,
        SchoolCoreDbContext context)
    {
        var assignment = await context.Assignments.FirstOrDefaultAsync(a => a.ExternalId == id);
        if (assignment == null)
        {
            return Results.NotFound();
        }

        context.Assignments.Remove(assignment);
        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    public static async Task<IResult> SubmitAssignment(
        Guid id,
        CreateAssignmentSubmissionRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var assignment = await context.Assignments.FirstOrDefaultAsync(a => a.ExternalId == id);
        if (assignment == null)
        {
            return Results.BadRequest("Assignment not found.");
        }

        var student = await context.Students.FirstOrDefaultAsync(s => s.ExternalId == request.StudentExternalId);
        if (student == null)
        {
            return Results.BadRequest("Student not found.");
        }

        var submission = new AssignmentSubmission
        {
            Id = Guid.NewGuid(),
            ExternalId = Guid.NewGuid(),
            AssignmentId = assignment.Id,
            StudentId = student.Id,
            FileName = request.FileName,
            SubmittedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(), // Replace with actual user ID
            UpdatedBy = Guid.NewGuid() // Replace with actual user ID
        };

        context.AssignmentSubmissions.Add(submission);
        await context.SaveChangesAsync();

        return Results.Created($"/api/v1/assignments/{assignment.ExternalId}/submit/{submission.ExternalId}", mapper.Map<AssignmentSubmissionResponse>(submission));
    }
}