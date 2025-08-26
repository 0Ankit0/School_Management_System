using Microsoft.EntityFrameworkCore;
using SMS.Contracts.Schedules;
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

public class ScheduleEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/schedules", GetSchedules)
            .WithName("GetSchedules");

        app.MapPost("/api/v1/schedules", PostSchedule)
            .WithName("CreateSchedule");

        app.MapPut("/api/v1/schedules/{id}", UpdateSchedule)
            .WithName("UpdateSchedule");

        app.MapDelete("/api/v1/schedules/{id}", DeleteSchedule)
            .WithName("DeleteSchedule");
    }

    public static async Task<IResult> GetSchedules(
        SchoolCoreDbContext context,
        IMapper mapper,
        IConfiguration configuration,
        [FromQuery] Guid? classId = null,
        [FromQuery] Guid? teacherId = null,
        [FromQuery] Guid? subjectId = null,
        [FromQuery] DateTime? date = null,
        [FromQuery] Guid? termId = null)
    {
        using IDbConnection dbConnection = new NpgsqlConnection(configuration.GetConnectionString("SchoolCoreConnection"));
        var sql = "SELECT s.\"ExternalId\" as Id, c.\"ExternalId\" as ClassExternalId, c.\"Name\" as ClassName, sub.\"ExternalId\" as SubjectExternalId, sub.\"Name\" as SubjectName, t.\"ExternalId\" as TeacherExternalId, t.\"FirstName\" || ' ' || t.\"LastName\" as TeacherFullName, term.\"ExternalId\" as TermExternalId, term.\"Name\" as TermName, s.\"Room\", s.\"DayOfWeek\", s.\"StartTime\", s.\"EndTime\", s.\"StartDate\", s.\"EndDate\" FROM \"Schedules\" s JOIN \"Classes\" c ON s.\"ClassId\" = c.\"Id\" JOIN \"Subjects\" sub ON s.\"SubjectId\" = sub.\"Id\" JOIN \"Teachers\" t ON s.\"TeacherId\" = t.\"Id\" LEFT JOIN \"Terms\" term ON s.\"TermId\" = term.\"Id\" WHERE 1=1";
        
        if (classId.HasValue)
        {
            sql += " AND c.\"ExternalId\" = @ClassId";
        }
        if (teacherId.HasValue)
        {
            sql += " AND t.\"ExternalId\" = @TeacherId";
        }
        if (subjectId.HasValue)
        {
            sql += " AND sub.\"ExternalId\" = @SubjectId";
        }
        if (termId.HasValue)
        {
            sql += " AND term.\"ExternalId\" = @TermId";
        }
        // Add date filtering if needed

        var schedules = await dbConnection.QueryAsync<ScheduleResponse>(sql, new { ClassId = classId, TeacherId = teacherId, SubjectId = subjectId, TermId = termId });
        return Results.Ok(schedules);
    }

    public static async Task<IResult> PostSchedule(
        CreateScheduleRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var classEntity = await context.Classes.FirstOrDefaultAsync(c => c.ExternalId == request.ClassExternalId);
        if (classEntity == null)
        {
            return Results.BadRequest("Class not found.");
        }

        var subject = await context.Subjects.FirstOrDefaultAsync(s => s.ExternalId == request.SubjectExternalId);
        if (subject == null)
        {
            return Results.BadRequest("Subject not found.");
        }

        var teacher = await context.Teachers.FirstOrDefaultAsync(t => t.ExternalId == request.TeacherExternalId);
        if (teacher == null)
        {
            return Results.BadRequest("Teacher not found.");
        }

        Term? term = null;
        if (request.TermExternalId.HasValue)
        {
            term = await context.Terms.FirstOrDefaultAsync(t => t.ExternalId == request.TermExternalId);
            if (term == null)
            {
                return Results.BadRequest("Term not found.");
            }
        }

        var schedule = new Schedule
        {
            Id = Guid.NewGuid(),
            ExternalId = Guid.NewGuid(),
            ClassId = classEntity.Id,
            SubjectId = subject.Id,
            TeacherId = teacher.Id,
            TermId = term?.Id,
            Room = request.Room,
            DayOfWeek = request.DayOfWeek,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(), // Replace with actual user ID
            UpdatedBy = Guid.NewGuid() // Replace with actual user ID
        };

        context.Schedules.Add(schedule);
        await context.SaveChangesAsync();

        return Results.Created($"/api/v1/schedules/{schedule.ExternalId}", mapper.Map<ScheduleResponse>(schedule));
    }

    public static async Task<IResult> UpdateSchedule(
        Guid id,
        UpdateScheduleRequest request,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var schedule = await context.Schedules.FirstOrDefaultAsync(s => s.ExternalId == id);
        if (schedule == null)
        {
            return Results.NotFound();
        }

        if (!string.IsNullOrEmpty(request.Room))
        {
            schedule.Room = request.Room;
        }
        if (request.DayOfWeek.HasValue)
        {
            schedule.DayOfWeek = request.DayOfWeek.Value;
        }
        if (request.StartTime.HasValue)
        {
            schedule.StartTime = request.StartTime.Value;
        }
        if (request.EndTime.HasValue)
        {
            schedule.EndTime = request.EndTime.Value;
        }
        if (request.StartDate.HasValue)
        {
            schedule.StartDate = request.StartDate.Value;
        }
        if (request.EndDate.HasValue)
        {
            schedule.EndDate = request.EndDate.Value;
        }
        schedule.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteSchedule(
        Guid id,
        SchoolCoreDbContext context)
    {
        var schedule = await context.Schedules.FirstOrDefaultAsync(s => s.ExternalId == id);
        if (schedule == null)
        {
            return Results.NotFound();
        }

        context.Schedules.Remove(schedule);
        await context.SaveChangesAsync();
        return Results.NoContent();
    }
}
