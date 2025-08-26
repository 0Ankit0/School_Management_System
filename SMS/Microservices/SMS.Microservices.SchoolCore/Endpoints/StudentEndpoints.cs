using Microsoft.EntityFrameworkCore;
using SMS.Contracts.Students;
using SMS.Microservices.SchoolCore.Data;
using SMS.Microservices.SchoolCore.Models;
using SMS.ServiceDefaults;

namespace SMS.Microservices.SchoolCore.Endpoints;

public class StudentEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/students", async (SchoolCoreDbContext dbContext) =>
        {
            var students = await dbContext.Students.ToListAsync();
            return Results.Ok(students);
        });

        app.MapPost("/api/students", async (CreateStudentRequest request, SchoolCoreDbContext dbContext) =>
        {
            var student = new Student
            {
                Id = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                Address = request.Address,
                Phone = request.Phone,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = Guid.NewGuid(), // Replace with actual user ID
                UpdatedBy = Guid.NewGuid() // Replace with actual user ID
            };

            await dbContext.Students.AddAsync(student);
            await dbContext.SaveChangesAsync();

            return Results.Created($"/api/students/{student.Id}", null);
        });
    }
}