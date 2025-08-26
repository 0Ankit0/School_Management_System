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
using SMS.Contracts.Teachers;

namespace SMS.Microservices.SchoolCore.Endpoints;

public class TeacherEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/teachers", GetTeachers)
            .WithName("GetTeachers");

        app.MapGet("/api/v1/teachers/{id}", GetTeacher)
            .WithName("GetTeacher");

        app.MapPost("/api/v1/teachers", PostTeacher)
            .WithName("CreateTeacher");

        app.MapPut("/api/v1/teachers/{id}", UpdateTeacher)
            .WithName("UpdateTeacher");

        app.MapDelete("/api/v1/teachers/{id}", DeleteTeacher)
            .WithName("DeleteTeacher");

        app.MapGet("/api/teachers/domain/{emailDomain}", GetTeachersByEmailDomain)
            .WithName("GetTeachersByEmailDomain");
    }

    public static async Task<IResult> GetTeachers(
        SchoolCoreDbContext context,
        IMapper mapper,
        IConfiguration configuration)
    {
        using IDbConnection dbConnection = new NpgsqlConnection(configuration.GetConnectionString("SchoolCoreConnection"));
        var sql = "SELECT * FROM \"Teachers\"";
        var teachers = await dbConnection.QueryAsync<Teacher>(sql);
        return Results.Ok(mapper.Map<IEnumerable<TeacherResponse>>(teachers));
    }

    public static async Task<IResult> GetTeacher(
        Guid id,
        SchoolCoreDbContext context,
        IMapper mapper,
        IConfiguration configuration)
    {
        using IDbConnection dbConnection = new NpgsqlConnection(configuration.GetConnectionString("SchoolCoreConnection"));
        var sql = "SELECT * FROM \"Teachers\" WHERE \"ExternalId\" = @Id";
        var teacher = await dbConnection.QueryFirstOrDefaultAsync<Teacher>(sql, new { Id = id });

        if (teacher == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(mapper.Map<TeacherResponse>(teacher));
    }

    public static async Task<IResult> PostTeacher(
        CreateTeacherRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var teacher = new Teacher
        {
            Id = Guid.NewGuid(),
            ExternalId = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            Department = request.Department,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(), // Replace with actual user ID
            UpdatedBy = Guid.NewGuid() // Replace with actual user ID
        };

        context.Teachers.Add(teacher);
        await context.SaveChangesAsync();

        return Results.Created($"/api/v1/teachers/{teacher.ExternalId}", mapper.Map<TeacherResponse>(teacher));
    }

    public static async Task<IResult> UpdateTeacher(
        Guid id,
        UpdateTeacherRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var teacher = await context.Teachers.FirstOrDefaultAsync(t => t.ExternalId == id);
        if (teacher == null)
        {
            return Results.NotFound();
        }

        if (!string.IsNullOrEmpty(request.FirstName))
        {
            teacher.FirstName = request.FirstName;
        }
        if (!string.IsNullOrEmpty(request.LastName))
        {
            teacher.LastName = request.LastName;
        }
        if (!string.IsNullOrEmpty(request.Email))
        {
            teacher.Email = request.Email;
        }
        if (!string.IsNullOrEmpty(request.Phone))
        {
            teacher.Phone = request.Phone;
        }
        if (!string.IsNullOrEmpty(request.Department))
        {
            teacher.Department = request.Department;
        }
        teacher.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteTeacher(
        Guid id,
        SchoolCoreDbContext context)
    {
        var teacher = await context.Teachers.FirstOrDefaultAsync(t => t.ExternalId == id);
        if (teacher == null)
        {
            return Results.NotFound();
        }

        context.Teachers.Remove(teacher);
        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    public static async Task<IResult> GetTeachersByEmailDomain(
        string emailDomain,
        IConfiguration configuration)
    {
        using IDbConnection dbConnection = new NpgsqlConnection(configuration.GetConnectionString("SchoolCoreConnection"));
        var sql = "SELECT * FROM \"Teachers\" WHERE \"Email\" LIKE @EmailDomain";
        var teachers = await dbConnection.QueryAsync<Teacher>(sql, new { EmailDomain = "%@" + emailDomain });
        return Results.Ok(teachers); // Assuming Teacher is already the DTO or will be mapped later
    }
}
