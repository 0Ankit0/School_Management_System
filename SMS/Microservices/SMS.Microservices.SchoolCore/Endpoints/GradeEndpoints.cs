using Microsoft.EntityFrameworkCore;
using SMS.Contracts.Grades;
using SMS.Microservices.SchoolCore.Data;
using SMS.Microservices.SchoolCore.Models;
using SMS.ServiceDefaults;
using Microsoft.AspNetCore.Routing;
using AutoMapper;
using Dapper;
using System.Data;
using Npgsql;
using Microsoft.AspNetCore.Mvc;

namespace SMS.Microservices.SchoolCore.Endpoints;

public class GradeEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/grades", GetGrades)
            .WithName("GetGrades");

        app.MapPost("/api/v1/grades", PostGrade)
            .WithName("CreateGrade");

        app.MapPut("/api/v1/grades/{id}", UpdateGrade)
            .WithName("UpdateGrade");

        app.MapDelete("/api/v1/grades/{id}", DeleteGrade)
            .WithName("DeleteGrade");
    }

    public static async Task<IResult> GetGrades(
        SchoolCoreDbContext context,
        IMapper mapper,
        IConfiguration configuration,
        [FromQuery] Guid? courseId = null,
        [FromQuery] Guid? studentId = null,
        [FromQuery] Guid? termId = null,
        [FromQuery] Guid? subjectId = null)
    {
        using IDbConnection dbConnection = new NpgsqlConnection(configuration.GetConnectionString("SchoolCoreConnection"));
        var sql = "SELECT g.\"ExternalId\" as Id, e.\"ExternalId\" as EnrollmentExternalId, a.\"ExternalId\" as AssignmentExternalId, s.\"ExternalId\" as StudentExternalId, s.\"FirstName\" || ' ' || s.\"LastName\" as StudentFullName, c.\"ExternalId\" as CourseExternalId, c.\"Title\" as CourseTitle, t.\"ExternalId\" as TermExternalId, t.\"Name\" as TermName, g.\"Score\", g.\"MaxScore\", g.\"Letter\", g.\"Weight\", g.\"Comment\", g.\"RecordedAt\" FROM \"Grades\" g LEFT JOIN \"Enrollments\" e ON g.\"EnrollmentId\" = e.\"Id\" LEFT JOIN \"Assignments\" a ON g.\"AssignmentId\" = a.\"Id\" JOIN \"Students\" s ON g.\"StudentId\" = s.\"Id\" LEFT JOIN \"Courses\" c ON g.\"CourseId\" = c.\"Id\" LEFT JOIN \"Terms\" t ON g.\"TermId\" = t.\"Id\" WHERE 1=1";
        
        if (courseId.HasValue)
        {
            sql += " AND c.\"ExternalId\" = @CourseId";
        }
        if (studentId.HasValue)
        {
            sql += " AND s.\"ExternalId\" = @StudentId";
        }
        if (termId.HasValue)
        {
            sql += " AND t.\"ExternalId\" = @TermId";
        }
        if (subjectId.HasValue)
        {
            // This would require joining with Courses and then Subjects, or having SubjectId directly in Grades
            // For now, assuming SubjectId is not directly in Grades for filtering
        }

        var grades = await dbConnection.QueryAsync<GradeResponse>(sql, new { CourseId = courseId, StudentId = studentId, TermId = termId });
        return Results.Ok(grades);
    }

    public static async Task<IResult> PostGrade(
        CreateGradeRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        Guid? enrollmentInternalId = null;
        if (request.EnrollmentExternalId.HasValue)
        {
            var enrollment = await context.Enrollments.FirstOrDefaultAsync(e => e.ExternalId == request.EnrollmentExternalId);
            if (enrollment == null)
            {
                return Results.BadRequest("Enrollment not found.");
            }
            enrollmentInternalId = enrollment.Id;
        }

        Guid? assignmentInternalId = null;
        if (request.AssignmentExternalId.HasValue)
        {
            var assignment = await context.Assignments.FirstOrDefaultAsync(a => a.ExternalId == request.AssignmentExternalId);
            if (assignment == null)
            {
                return Results.BadRequest("Assignment not found.");
            }
            assignmentInternalId = assignment.Id;
        }

        var student = await context.Students.FirstOrDefaultAsync(s => s.ExternalId == request.StudentExternalId);
        if (student == null)
        {
            return Results.BadRequest("Student not found.");
        }

        Guid? courseInternalId = null;
        if (request.CourseExternalId.HasValue)
        {
            var course = await context.Courses.FirstOrDefaultAsync(c => c.ExternalId == request.CourseExternalId);
            if (course == null)
            {
                return Results.BadRequest("Course not found.");
            }
            courseInternalId = course.Id;
        }

        Guid? termInternalId = null;
        if (request.TermExternalId.HasValue)
        {
            var term = await context.Terms.FirstOrDefaultAsync(t => t.ExternalId == request.TermExternalId);
            if (term == null)
            {
                return Results.BadRequest("Term not found.");
            }
            termInternalId = term.Id;
        }

        var grade = new Grade
        {
            Id = Guid.NewGuid(),
            ExternalId = Guid.NewGuid(),
            EnrollmentId = enrollmentInternalId,
            AssignmentId = assignmentInternalId,
            StudentId = student.Id,
            CourseId = courseInternalId,
            TermId = termInternalId,
            Score = request.Score,
            MaxScore = request.MaxScore,
            Letter = request.Letter,
            Weight = request.Weight,
            Comment = request.Comment,
            RecordedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(), // Replace with actual user ID
            UpdatedBy = Guid.NewGuid() // Replace with actual user ID
        };

        context.Grades.Add(grade);
        await context.SaveChangesAsync();

        return Results.Created($"/api/v1/grades/{grade.ExternalId}", mapper.Map<GradeResponse>(grade));
    }

    public static async Task<IResult> UpdateGrade(
        Guid id,
        UpdateGradeRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var grade = await context.Grades.FirstOrDefaultAsync(g => g.ExternalId == id);
        if (grade == null)
        {
            return Results.NotFound();
        }

        if (request.Score.HasValue)
        {
            grade.Score = request.Score.Value;
        }
        if (request.MaxScore.HasValue)
        {
            grade.MaxScore = request.MaxScore.Value;
        }
        if (!string.IsNullOrEmpty(request.Letter))
        {
            grade.Letter = request.Letter;
        }
        if (request.Weight.HasValue)
        {
            grade.Weight = request.Weight.Value;
        }
        if (!string.IsNullOrEmpty(request.Comment))
        {
            grade.Comment = request.Comment;
        }
        grade.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteGrade(
        Guid id,
        SchoolCoreDbContext context)
    {
        var grade = await context.Grades.FirstOrDefaultAsync(g => g.ExternalId == id);
        if (grade == null)
        {
            return Results.NotFound();
        }

        context.Grades.Remove(grade);
        await context.SaveChangesAsync();
        return Results.NoContent();
    }
}
