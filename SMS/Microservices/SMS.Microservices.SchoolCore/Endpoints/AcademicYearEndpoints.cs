using Microsoft.EntityFrameworkCore;
using SMS.Contracts.AcademicYears;
using SMS.Microservices.SchoolCore.Data;
using SMS.Microservices.SchoolCore.Models;
using SMS.ServiceDefaults;
using Microsoft.AspNetCore.Routing;
using AutoMapper;
using Dapper;
using System.Data;
using Npgsql;

namespace SMS.Microservices.SchoolCore.Endpoints;

public class AcademicYearEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/academic-years", GetAcademicYears)
            .WithName("GetAcademicYears");

        app.MapPost("/api/v1/academic-years", PostAcademicYear)
            .WithName("CreateAcademicYear");

        app.MapPut("/api/v1/academic-years/{id}", UpdateAcademicYear)
            .WithName("UpdateAcademicYear");

        app.MapDelete("/api/v1/academic-years/{id}", DeleteAcademicYear)
            .WithName("DeleteAcademicYear");
    }

    public static async Task<IResult> GetAcademicYears(
        SchoolCoreDbContext context,
        IMapper mapper,
        IConfiguration configuration)
    {
        using IDbConnection dbConnection = new NpgsqlConnection(configuration.GetConnectionString("SchoolCoreConnection"));
        var sql = "SELECT \"ExternalId\" as Id, \"Name\", \"StartDate\", \"EndDate\", \"IsActive\" FROM \"AcademicYears\"";
        var academicYears = await dbConnection.QueryAsync<AcademicYearResponse>(sql);
        return Results.Ok(academicYears);
    }

    public static async Task<IResult> PostAcademicYear(
        CreateAcademicYearRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var academicYear = new AcademicYear
        {
            Id = Guid.NewGuid(),
            ExternalId = Guid.NewGuid(),
            Name = request.Name,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(), // Replace with actual user ID
            UpdatedBy = Guid.NewGuid() // Replace with actual user ID
        };

        context.AcademicYears.Add(academicYear);
        await context.SaveChangesAsync();

        return Results.Created($"/api/v1/academic-years/{academicYear.ExternalId}", mapper.Map<AcademicYearResponse>(academicYear));
    }

    public static async Task<IResult> UpdateAcademicYear(
        Guid id,
        UpdateAcademicYearRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var academicYear = await context.AcademicYears.FirstOrDefaultAsync(a => a.ExternalId == id);
        if (academicYear == null)
        {
            return Results.NotFound();
        }

        if (!string.IsNullOrEmpty(request.Name))
        {
            academicYear.Name = request.Name;
        }
        if (request.StartDate.HasValue)
        {
            academicYear.StartDate = request.StartDate.Value;
        }
        if (request.EndDate.HasValue)
        {
            academicYear.EndDate = request.EndDate.Value;
        }
        if (request.IsActive.HasValue)
        {
            academicYear.IsActive = request.IsActive.Value;
        }
        academicYear.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteAcademicYear(
        Guid id,
        SchoolCoreDbContext context)
    {
        var academicYear = await context.AcademicYears.FirstOrDefaultAsync(a => a.ExternalId == id);
        if (academicYear == null)
        {
            return Results.NotFound();
        }

        context.AcademicYears.Remove(academicYear);
        await context.SaveChangesAsync();
        return Results.NoContent();
    }
}
