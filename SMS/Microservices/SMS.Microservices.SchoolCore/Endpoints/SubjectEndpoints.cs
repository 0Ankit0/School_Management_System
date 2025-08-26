using Microsoft.EntityFrameworkCore;
using SMS.Contracts.Subjects;
using SMS.Microservices.SchoolCore.Data;
using SMS.Microservices.SchoolCore.Models;
using SMS.ServiceDefaults;
using Microsoft.AspNetCore.Routing;
using AutoMapper;
using Dapper;
using System.Data;
using Npgsql;

namespace SMS.Microservices.SchoolCore.Endpoints;

public class SubjectEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/subjects", GetSubjects)
            .WithName("GetSubjects");

        app.MapPost("/api/v1/subjects", PostSubject)
            .WithName("CreateSubject");

        app.MapPut("/api/v1/subjects/{id}", UpdateSubject)
            .WithName("UpdateSubject");

        app.MapDelete("/api/v1/subjects/{id}", DeleteSubject)
            .WithName("DeleteSubject");
    }

    public static async Task<IResult> GetSubjects(
        SchoolCoreDbContext context,
        IMapper mapper,
        IConfiguration configuration)
    {
        using IDbConnection dbConnection = new NpgsqlConnection(configuration.GetConnectionString("SchoolCoreConnection"));
        var sql = "SELECT \"ExternalId\" as Id, \"Code\", \"Name\", \"Description\" FROM \"Subjects\"";
        var subjects = await dbConnection.QueryAsync<SubjectResponse>(sql);
        return Results.Ok(subjects);
    }

    public static async Task<IResult> PostSubject(
        CreateSubjectRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var subject = new Subject
        {
            Id = Guid.NewGuid(),
            ExternalId = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(), // Replace with actual user ID
            UpdatedBy = Guid.NewGuid() // Replace with actual user ID
        };

        context.Subjects.Add(subject);
        await context.SaveChangesAsync();

        return Results.Created($"/api/v1/subjects/{subject.ExternalId}", mapper.Map<SubjectResponse>(subject));
    }

    public static async Task<IResult> UpdateSubject(
        Guid id,
        UpdateSubjectRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var subject = await context.Subjects.FirstOrDefaultAsync(s => s.ExternalId == id);
        if (subject == null)
        {
            return Results.NotFound();
        }

        if (!string.IsNullOrEmpty(request.Code))
        {
            subject.Code = request.Code;
        }
        if (!string.IsNullOrEmpty(request.Name))
        {
            subject.Name = request.Name;
        }
        if (!string.IsNullOrEmpty(request.Description))
        {
            subject.Description = request.Description;
        }
        subject.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteSubject(
        Guid id,
        SchoolCoreDbContext context)
    {
        var subject = await context.Subjects.FirstOrDefaultAsync(s => s.ExternalId == id);
        if (subject == null)
        {
            return Results.NotFound();
        }

        context.Subjects.Remove(subject);
        await context.SaveChangesAsync();
        return Results.NoContent();
    }
}
