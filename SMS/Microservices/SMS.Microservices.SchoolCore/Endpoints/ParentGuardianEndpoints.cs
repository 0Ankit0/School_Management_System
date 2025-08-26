using Microsoft.EntityFrameworkCore;
using SMS.Contracts.ParentGuardians;
using SMS.Microservices.SchoolCore.Data;
using SMS.Microservices.SchoolCore.Models;
using SMS.ServiceDefaults;
using Microsoft.AspNetCore.Routing;
using AutoMapper;
using Dapper;
using System.Data;
using Npgsql;
using SMS.Contracts.Students;

namespace SMS.Microservices.SchoolCore.Endpoints;

public class ParentGuardianEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/parents", GetParentGuardians)
            .WithName("GetParentGuardians");

        app.MapPost("/api/v1/parents", PostParentGuardian)
            .WithName("CreateParentGuardian");

        app.MapPut("/api/v1/parents/{id}", UpdateParentGuardian)
            .WithName("UpdateParentGuardian");

        app.MapDelete("/api/v1/parents/{id}", DeleteParentGuardian)
            .WithName("DeleteParentGuardian");

        app.MapGet("/api/v1/parents/{id}/children", GetChildrenForParent)
            .WithName("GetChildrenForParent");
    }

    public static async Task<IResult> GetParentGuardians(
        SchoolCoreDbContext context,
        IMapper mapper,
        IConfiguration configuration)
    {
        using IDbConnection dbConnection = new NpgsqlConnection(configuration.GetConnectionString("SchoolCoreConnection"));
        var sql = "SELECT \"ExternalId\" as Id, \"FirstName\", \"LastName\", \"Email\", \"Phone\" FROM \"ParentGuardians\"";
        var parentGuardians = await dbConnection.QueryAsync<ParentGuardianResponse>(sql);
        return Results.Ok(parentGuardians);
    }

    public static async Task<IResult> PostParentGuardian(
        CreateParentGuardianRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var parentGuardian = new ParentGuardian
        {
            Id = Guid.NewGuid(),
            ExternalId = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(), // Replace with actual user ID
            UpdatedBy = Guid.NewGuid() // Replace with actual user ID
        };

        context.ParentGuardians.Add(parentGuardian);
        await context.SaveChangesAsync();

        return Results.Created($"/api/v1/parents/{parentGuardian.ExternalId}", mapper.Map<ParentGuardianResponse>(parentGuardian));
    }

    public static async Task<IResult> UpdateParentGuardian(
        Guid id,
        UpdateParentGuardianRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var parentGuardian = await context.ParentGuardians.FirstOrDefaultAsync(p => p.ExternalId == id);
        if (parentGuardian == null)
        {
            return Results.NotFound();
        }

        if (!string.IsNullOrEmpty(request.FirstName))
        {
            parentGuardian.FirstName = request.FirstName;
        }
        if (!string.IsNullOrEmpty(request.LastName))
        {
            parentGuardian.LastName = request.LastName;
        }
        if (!string.IsNullOrEmpty(request.Email))
        {
            parentGuardian.Email = request.Email;
        }
        if (!string.IsNullOrEmpty(request.Phone))
        {
            parentGuardian.Phone = request.Phone;
        }
        parentGuardian.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteParentGuardian(
        Guid id,
        SchoolCoreDbContext context)
    {
        var parentGuardian = await context.ParentGuardians.FirstOrDefaultAsync(p => p.ExternalId == id);
        if (parentGuardian == null)
        {
            return Results.NotFound();
        }

        context.ParentGuardians.Remove(parentGuardian);
        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    public static async Task<IResult> GetChildrenForParent(
        Guid id,
        SchoolCoreDbContext context,
        IMapper mapper,
        IConfiguration configuration)
    {
        using IDbConnection dbConnection = new NpgsqlConnection(configuration.GetConnectionString("SchoolCoreConnection"));
        var sql = "SELECT s.\"ExternalId\" as Id, s.\"FirstName\", s.\"LastName\", s.\"DateOfBirth\", s.\"Gender\", s.\"Address\", s.\"Phone\", s.\"Email\" FROM \"Students\" s JOIN \"StudentParents\" sp ON s.\"Id\" = sp.\"StudentId\" WHERE sp.\"ParentId\" = (SELECT \"Id\" FROM \"ParentGuardians\" WHERE \"ExternalId\" = @ParentId)";
        var students = await dbConnection.QueryAsync<StudentResponse>(sql, new { ParentId = id });
        return Results.Ok(students);
    }
}

