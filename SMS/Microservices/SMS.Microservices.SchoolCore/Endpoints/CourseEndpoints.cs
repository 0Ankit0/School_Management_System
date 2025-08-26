using Microsoft.EntityFrameworkCore;
using SMS.Contracts.Courses;
using SMS.Microservices.SchoolCore.Data;
using SMS.Microservices.SchoolCore.Models;
using SMS.ServiceDefaults;

namespace SMS.Microservices.SchoolCore.Endpoints;

public class CourseEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/courses", async (SchoolCoreDbContext dbContext) =>
        {
            var courses = await dbContext.Courses.ToListAsync();
            return Results.Ok(courses);
        });

        app.MapPost("/api/courses", async (CreateCourseRequest request, SchoolCoreDbContext dbContext) =>
        {
            var course = new Course
            {
                Id = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                CourseCode = request.CourseCode,
                Title = request.Title,
                Description = request.Description,
                Credits = request.Credits,
                TeacherId = request.TeacherExternalId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = Guid.NewGuid(), // Replace with actual user ID
                UpdatedBy = Guid.NewGuid() // Replace with actual user ID
            };

            await dbContext.Courses.AddAsync(course);
            await dbContext.SaveChangesAsync();

            return Results.Created($"/api/courses/{course.Id}", null);
        });
    }
}