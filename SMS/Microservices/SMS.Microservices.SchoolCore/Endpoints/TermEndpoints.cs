using Microsoft.EntityFrameworkCore;
using SMS.Contracts.Terms;
using SMS.Microservices.SchoolCore.Data;
using SMS.Microservices.SchoolCore.Models;
using SMS.ServiceDefaults;
using Microsoft.AspNetCore.Routing;
using AutoMapper;
using Dapper;
using System.Data;
using Npgsql;

namespace SMS.Microservices.SchoolCore.Endpoints;

public class TermEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/terms", GetTerms)
            .WithName("GetTerms");

        app.MapPost("/api/v1/terms", PostTerm)
            .WithName("CreateTerm");

        app.MapPut("/api/v1/terms/{id}", UpdateTerm)
            .WithName("UpdateTerm");

        app.MapDelete("/api/v1/terms/{id}", DeleteTerm)
            .WithName("DeleteTerm");
    }

    public static async Task<IResult> GetTerms(
        SchoolCoreDbContext context,
        IMapper mapper,
        IConfiguration configuration)
    {
        using IDbConnection dbConnection = new NpgsqlConnection(configuration.GetConnectionString("SchoolCoreConnection"));
        var sql = "SELECT t.\"ExternalId\" as Id, t.\"Name\", t.\"StartDate\", t.\"EndDate\", ay.\"ExternalId\" as AcademicYearExternalId, ay.\"Name\" as AcademicYearName FROM \"Terms\" t JOIN \"AcademicYears\" ay ON t.\"AcademicYearId\" = ay.\"Id\"";
        var terms = await dbConnection.QueryAsync<TermResponse>(sql);
        return Results.Ok(terms);
    }

    public static async Task<IResult> PostTerm(
        CreateTermRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var academicYear = await context.AcademicYears.FirstOrDefaultAsync(ay => ay.ExternalId == request.AcademicYearExternalId);
        if (academicYear == null)
        {
            return Results.BadRequest("Academic Year not found.");
        }

        var term = new Term
        {
            Id = Guid.NewGuid(),
            ExternalId = Guid.NewGuid(),
            AcademicYearId = academicYear.Id,
            Name = request.Name,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(), // Replace with actual user ID
            UpdatedBy = Guid.NewGuid() // Replace with actual user ID
        };

        context.Terms.Add(term);
        await context.SaveChangesAsync();

        return Results.Created($"/api/v1/terms/{term.ExternalId}", mapper.Map<TermResponse>(term));
    }

    public static async Task<IResult> UpdateTerm(
        Guid id,
        UpdateTermRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var term = await context.Terms.FirstOrDefaultAsync(t => t.ExternalId == id);
        if (term == null)
        {
            return Results.NotFound();
        }

        if (!string.IsNullOrEmpty(request.Name))
        {
            term.Name = request.Name;
        }
        if (request.StartDate.HasValue)
        {
            term.StartDate = request.StartDate.Value;
        }
        if (request.EndDate.HasValue)
        {
            term.EndDate = request.EndDate.Value;
        }
        term.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteTerm(
        Guid id,
        SchoolCoreDbContext context)
    {
        var term = await context.Terms.FirstOrDefaultAsync(t => t.ExternalId == id);
        if (term == null)
        {
            return Results.NotFound();
        }

        context.Terms.Remove(term);
        await context.SaveChangesAsync();
        return Results.NoContent();
    }
}
