using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMS.Contracts.Reporting;
using SMS.Microservices.Reporting.Data;
using SMS.Microservices.Reporting.Models;
using SMS.ServiceDefaults;

namespace SMS.Microservices.Reporting.Endpoints;

public class ReportingEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/reporting/reports", async (ReportingDbContext db, IMapper mapper) =>
        {
            return await db.Reports.Select(r => mapper.Map<ReportResponse>(r)).ToListAsync();
        });

        app.MapGet("/api/reporting/reports/{id}", async (Guid id, ReportingDbContext db, IMapper mapper) =>
        {
            var report = await db.Reports.FirstOrDefaultAsync(r => r.Id == id);
            return report == null ? Results.NotFound() : Results.Ok(mapper.Map<ReportResponse>(report));
        });

        app.MapPost("/api/reporting/reports", async (CreateReportRequest request, ReportingDbContext db, IMapper mapper) =>
        {
            var report = mapper.Map<Report>(request);
            report.Id = Guid.NewGuid();
            report.CreatedAt = DateTime.UtcNow;
            report.UpdatedAt = DateTime.UtcNow;
            // TODO: Get CreatedBy and UpdatedBy from authenticated user
            report.CreatedBy = Guid.Empty;
            report.UpdatedBy = Guid.Empty;

            db.Reports.Add(report);
            await db.SaveChangesAsync();

            return Results.Created($"/api/reporting/reports/{report.Id}", mapper.Map<ReportResponse>(report));
        });

        app.MapPut("/api/reporting/reports/{id}", async (Guid id, UpdateReportRequest request, ReportingDbContext db, IMapper mapper) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest();
            }

            var report = await db.Reports.FirstOrDefaultAsync(r => r.Id == id);
            if (report == null)
            {
                return Results.NotFound();
            }

            mapper.Map(request, report);
            report.UpdatedAt = DateTime.UtcNow;
            // TODO: Get UpdatedBy from authenticated user
            report.UpdatedBy = Guid.Empty;

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapDelete("/api/reporting/reports/{id}", async (Guid id, ReportingDbContext db) =>
        {
            var report = await db.Reports.FirstOrDefaultAsync(r => r.Id == id);
            if (report == null)
            {
                return Results.NotFound();
            }

            db.Reports.Remove(report);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}

