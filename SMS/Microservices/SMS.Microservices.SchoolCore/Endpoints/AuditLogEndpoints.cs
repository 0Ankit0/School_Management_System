using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.Microservices.SchoolCore.Data;
using SMS.Microservices.SchoolCore.Models;
using SMS.Contracts.AuditLogs;
using AutoMapper;
using Dapper;
using System.Data;
using Npgsql;
using SMS.ServiceDefaults;
using Microsoft.AspNetCore.Routing;

namespace SMS.Microservices.SchoolCore.Endpoints;

public class AuditLogEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/auditlogs", GetAuditLogs)
            .WithName("GetAuditLogs");

        app.MapGet("/api/auditlogs/{id}", GetAuditLog)
            .WithName("GetAuditLog");
    }

    public static async Task<IResult> GetAuditLogs(
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var auditLogs = await context.AuditLogs.ToListAsync();
        return Results.Ok(mapper.Map<IEnumerable<AuditLogResponse>>(auditLogs));
    }

    public static async Task<IResult> GetAuditLog(
        Guid id,
        SchoolCoreDbContext context,
        IMapper mapper)
    {
        var auditLog = await context.AuditLogs.FirstOrDefaultAsync(a => a.ExternalId == id);

        if (auditLog == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(mapper.Map<AuditLogResponse>(auditLog));
    }
}
