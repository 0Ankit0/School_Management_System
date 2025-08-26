using Microsoft.EntityFrameworkCore;
using SMS.Contracts.Classes;
using SMS.Microservices.SchoolCore.Data;
using SMS.Microservices.SchoolCore.Models;
using SMS.ServiceDefaults;
using Microsoft.AspNetCore.Routing;
using AutoMapper;
using Dapper;
using System.Data;
using Npgsql;

namespace SMS.Microservices.SchoolCore.Endpoints;

public class ClassEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/classes", GetClasses)
            .WithName("GetClasses");

        app.MapPost("/api/v1/classes", PostClass)
            .WithName("CreateClass");

        app.MapPut("/api/v1/classes/{id}", UpdateClass)
            .WithName("UpdateClass");

        app.MapDelete("/api/v1/classes/{id}", DeleteClass)
            .WithName("DeleteClass");
    }

    public static async Task<IResult> GetClasses(
        SchoolCoreDbContext context,
        IMapper mapper,
        IConfiguration configuration)
    {
        using IDbConnection dbConnection = new NpgsqlConnection(configuration.GetConnectionString("SchoolCoreConnection"));
        var sql = "SELECT c.\"ExternalId\" as Id, c.\"Name\", c.\"Capacity\", ay.\"ExternalId\" as AcademicYearExternalId, ay.\"Name\" as AcademicYearName, t.\"ExternalId\" as HomeroomTeacherExternalId, t.\"FirstName\" || ' ' || t.\"LastName\" as HomeroomTeacherFullName FROM \"Classes\" c JOIN \"AcademicYears\" ay ON c.\"AcademicYearId\" = ay.\"Id\" LEFT JOIN \"Teachers\" t ON c.\"HomeroomTeacherId\" = t.\"Id\"";
        var classes = await dbConnection.QueryAsync<ClassResponse>(sql);
        return Results.Ok(classes);
    }

    public static async Task<IResult> PostClass(
        CreateClassRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var academicYear = await context.AcademicYears.FirstOrDefaultAsync(ay => ay.ExternalId == request.AcademicYearExternalId);
        if (academicYear == null)
        {
            return Results.BadRequest("Academic Year not found.");
        }

        Teacher? homeroomTeacher = null;
        if (request.HomeroomTeacherExternalId.HasValue)
        {
            homeroomTeacher = await context.Teachers.FirstOrDefaultAsync(t => t.ExternalId == request.HomeroomTeacherExternalId);
            if (homeroomTeacher == null)
            {
                return Results.BadRequest("Homeroom Teacher not found.");
            }
        }

        var classEntity = new Class
        {
            Id = Guid.NewGuid(),
            ExternalId = Guid.NewGuid(),
            AcademicYearId = academicYear.Id,
            Name = request.Name,
            Capacity = request.Capacity,
            HomeroomTeacherId = homeroomTeacher?.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(), // Replace with actual user ID
            UpdatedBy = Guid.NewGuid() // Replace with actual user ID
        };

        context.Classes.Add(classEntity);
        await context.SaveChangesAsync();

        return Results.Created($"/api/v1/classes/{classEntity.ExternalId}", mapper.Map<ClassResponse>(classEntity));
    }

    public static async Task<IResult> UpdateClass(
        Guid id,
        UpdateClassRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var classEntity = await context.Classes.FirstOrDefaultAsync(c => c.ExternalId == id);
        if (classEntity == null)
        {
            return Results.NotFound();
        }

        if (!string.IsNullOrEmpty(request.Name))
        {
            classEntity.Name = request.Name;
        }
        if (request.Capacity.HasValue)
        {
            classEntity.Capacity = request.Capacity.Value;
        }
        if (request.HomeroomTeacherExternalId.HasValue)
        {
            var homeroomTeacher = await context.Teachers.FirstOrDefaultAsync(t => t.ExternalId == request.HomeroomTeacherExternalId);
            if (homeroomTeacher == null)
            {
                return Results.BadRequest("Homeroom Teacher not found.");
            }
            classEntity.HomeroomTeacherId = homeroomTeacher.Id;
        }
        classEntity.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteClass(
        Guid id,
        SchoolCoreDbContext context)
    {
        var classEntity = await context.Classes.FirstOrDefaultAsync(c => c.ExternalId == id);
        if (classEntity == null)
        {
            return Results.NotFound();
        }

        context.Classes.Remove(classEntity);
        await context.SaveChangesAsync();
        return Results.NoContent();
    }
}
