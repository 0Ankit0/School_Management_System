using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.Microservices.SchoolCore.Data;
using SMS.Microservices.SchoolCore.Models;
using AutoMapper;
using Dapper;
using System.Data;
using Npgsql;
using SMS.ServiceDefaults;

namespace SMS.Microservices.SchoolCore.Endpoints;

public class TeacherEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/teachers", GetTeachers)
            .WithName("GetTeachers");

        app.MapGet("/api/teachers/{id}", GetTeacher)
            .WithName("GetTeacher");

        app.MapPost("/api/teachers", PostTeacher)
            .WithName("CreateTeacher");

        app.MapGet("/api/teachers/domain/{emailDomain}", GetTeachersByEmailDomain)
            .WithName("GetTeachersByEmailDomain");
    }

    public static async Task<IResult> GetTeachers(
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var teachers = await context.Teachers.ToListAsync();
        return Results.Ok(mapper.Map<IEnumerable<Teacher>>(teachers)); // Assuming Teacher is already the DTO or will be mapped later
    }

    public static async Task<IResult> GetTeacher(
        Guid id,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var teacher = await context.Teachers.FirstOrDefaultAsync(t => t.ExternalId == id);

        if (teacher == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(mapper.Map<Teacher>(teacher)); // Assuming Teacher is already the DTO or will be mapped later
    }

    public static async Task<IResult> PostTeacher(
        Teacher teacher,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        context.Teachers.Add(teacher);
        await context.SaveChangesAsync();

        return Results.CreatedAtRoute("GetTeacher", new { id = teacher.ExternalId }, mapper.Map<Teacher>(teacher)); // Assuming Teacher is already the DTO or will be mapped later
    }

    public static async Task<IResult> GetTeachersByEmailDomain(
        string emailDomain,
        IConfiguration configuration)
    {
        using IDbConnection dbConnection = new NpgsqlConnection(configuration.GetConnectionString("SchoolCoreDbConnection"));
        var sql = "SELECT * FROM \"Teachers\" WHERE \"Email\" LIKE @EmailDomain";
        var teachers = await dbConnection.QueryAsync<Teacher>(sql, new { EmailDomain = "%@" + emailDomain });
        return Results.Ok(teachers); // Assuming Teacher is already the DTO or will be mapped later
    }
}
