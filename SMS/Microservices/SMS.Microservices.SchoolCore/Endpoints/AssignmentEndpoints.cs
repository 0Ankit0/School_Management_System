using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.Microservices.SchoolCore.Data;
using SMS.Microservices.SchoolCore.Models;
using SMS.Contracts.Assignments;
using AutoMapper;
using Dapper;
using System.Data;
using Npgsql;
using SMS.ServiceDefaults;

namespace SMS.Microservices.SchoolCore.Endpoints;

public class AssignmentEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/assignments", GetAssignments)
            .WithName("GetAssignments");

        app.MapGet("/api/assignments/{id}", GetAssignment)
            .WithName("GetAssignment");

        app.MapPost("/api/assignments", PostAssignment)
            .WithName("CreateAssignment");
    }

    public static async Task<IResult> GetAssignments(
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var assignments = await context.Assignments.ToListAsync();
        return Results.Ok(mapper.Map<IEnumerable<AssignmentResponse>>(assignments));
    }

    public static async Task<IResult> GetAssignment(
        Guid id,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var assignment = await context.Assignments.FirstOrDefaultAsync(a => a.ExternalId == id);

        if (assignment == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(mapper.Map<AssignmentResponse>(assignment));
    }

    public static async Task<IResult> PostAssignment(
        CreateAssignmentRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var assignment = mapper.Map<Assignment>(request);
        context.Assignments.Add(assignment);
        await context.SaveChangesAsync();

        return Results.CreatedAtRoute("GetAssignment", new { id = assignment.ExternalId }, mapper.Map<AssignmentResponse>(assignment));
    }
}
