using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.Microservices.SchoolCore.Data;
using SMS.Microservices.SchoolCore.Models;
using SMS.Contracts.Courses;
using AutoMapper;
using Dapper;
using System.Data;
using Npgsql;
using SMS.ServiceDefaults;

namespace SMS.Microservices.SchoolCore.Endpoints;

public class CourseEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/courses", GetCourses)
            .WithName("GetCourses");

        app.MapGet("/api/courses/{id}", GetCourse)
            .WithName("GetCourse");

        app.MapPost("/api/courses", PostCourse)
            .WithName("CreateCourse");

        app.MapGet("/api/courses/credits", GetCoursesByCredits)
            .WithName("GetCoursesByCredits");
    }

    public static async Task<IResult> GetCourses(
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var courses = await context.Courses.ToListAsync();
        return Results.Ok(mapper.Map<IEnumerable<CourseResponse>>(courses));
    }

    public static async Task<IResult> GetCourse(
        Guid id,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var course = await context.Courses.FirstOrDefaultAsync(c => c.ExternalId == id);

        if (course == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(mapper.Map<CourseResponse>(course));
    }

    public static async Task<IResult> PostCourse(
        CreateCourseRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var course = mapper.Map<Course>(request);
        context.Courses.Add(course);
        await context.SaveChangesAsync();

        return Results.CreatedAtRoute("GetCourse", new { id = course.ExternalId }, mapper.Map<CourseResponse>(course));
    }

    public static async Task<IResult> GetCoursesByCredits(
        int minCredits,
        int maxCredits,
        IConfiguration configuration)
    {
        using IDbConnection dbConnection = new NpgsqlConnection(configuration.GetConnectionString("SchoolCoreDbConnection"));
        var sql = "SELECT * FROM \"Courses\" WHERE \"Credits\" BETWEEN @MinCredits AND @MaxCredits";
        var courses = await dbConnection.QueryAsync<Course>(sql, new { MinCredits = minCredits, MaxCredits = maxCredits });
        // Note: Mapping to DTOs should happen here if Course is a domain model
        return Results.Ok(courses); // Assuming Course is already the DTO or will be mapped later
    }
}
