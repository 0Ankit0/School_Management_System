using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.Microservices.SchoolCore.Data;
using SMS.Microservices.SchoolCore.Models;
using SMS.Contracts.Students;
using AutoMapper;
using Dapper;
using System.Data;
using Npgsql;
using SMS.ServiceDefaults;

namespace SMS.Microservices.SchoolCore.Endpoints;

public class StudentEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/students", GetStudents)
            .WithName("GetStudents");

        app.MapGet("/api/students/{id}", GetStudent)
            .WithName("GetStudent");

        app.MapPost("/api/students", PostStudent)
            .WithName("CreateStudent");

        app.MapGet("/api/students/filter", GetFilteredStudents)
            .WithName("GetFilteredStudents");
    }

    public static async Task<IResult> GetStudents(
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var students = await context.Students.ToListAsync();
        return Results.Ok(mapper.Map<IEnumerable<StudentResponse>>(students));
    }

    public static async Task<IResult> GetStudent(
        Guid id,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var student = await context.Students.FirstOrDefaultAsync(s => s.ExternalId == id);

        if (student == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(mapper.Map<StudentResponse>(student));
    }

    public static async Task<IResult> PostStudent(
        CreateStudentRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var student = mapper.Map<Student>(request);
        context.Students.Add(student);
        await context.SaveChangesAsync();

        return Results.CreatedAtRoute("GetStudent", new { id = student.ExternalId }, mapper.Map<StudentResponse>(student));
    }

    public static async Task<IResult> GetFilteredStudents(
        string gender,
        int minAge,
        int maxAge,
        IConfiguration configuration)
    {
        using IDbConnection dbConnection = new NpgsqlConnection(configuration.GetConnectionString("SchoolCoreDbConnection"));
        var sql = "SELECT * FROM \"Students\" WHERE \"Gender\" = @Gender AND DATE_PART('year', AGE(\"DateOfBirth\")) BETWEEN @MinAge AND @MaxAge";
        var students = await dbConnection.QueryAsync<Student>(sql, new { Gender = gender, MinAge = minAge, MaxAge = maxAge });
        // Note: Mapping to DTOs should happen here if Student is a domain model
        return Results.Ok(students);
    }
}
