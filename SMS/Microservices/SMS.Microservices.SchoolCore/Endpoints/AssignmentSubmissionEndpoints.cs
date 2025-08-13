using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.Microservices.SchoolCore.Data;
using SMS.Microservices.SchoolCore.Models;
using SMS.Contracts.AssignmentSubmissions;
using AutoMapper;
using Dapper;
using System.Data;
using Npgsql;
using SMS.ServiceDefaults;

namespace SMS.Microservices.SchoolCore.Endpoints;

public class AssignmentSubmissionEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/assignmentsubmissions", GetAssignmentSubmissions)
            .WithName("GetAssignmentSubmissions");

        app.MapGet("/api/assignmentsubmissions/{id}", GetAssignmentSubmission)
            .WithName("GetAssignmentSubmission");

        app.MapPost("/api/assignmentsubmissions", PostAssignmentSubmission)
            .WithName("CreateAssignmentSubmission");
    }

    public static async Task<IResult> GetAssignmentSubmissions(
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var submissions = await context.AssignmentSubmissions.ToListAsync();
        return Results.Ok(mapper.Map<IEnumerable<AssignmentSubmissionResponse>>(submissions));
    }

    public static async Task<IResult> GetAssignmentSubmission(
        Guid id,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var submission = await context.AssignmentSubmissions.FirstOrDefaultAsync(s => s.ExternalId == id);

        if (submission == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(mapper.Map<AssignmentSubmissionResponse>(submission));
    }

    public static async Task<IResult> PostAssignmentSubmission(
        CreateAssignmentSubmissionRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var submission = mapper.Map<AssignmentSubmission>(request);
        context.AssignmentSubmissions.Add(submission);
        await context.SaveChangesAsync();

        return Results.CreatedAtRoute("GetAssignmentSubmission", new { id = submission.ExternalId }, mapper.Map<AssignmentSubmissionResponse>(submission));
    }
}
