using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMS.Contracts.Payroll;
using SMS.Microservices.Payroll.Data;
using SMS.Microservices.Payroll.Models;
using SMS.ServiceDefaults;

namespace SMS.Microservices.Payroll.Endpoints;

public class PayrollEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/payroll/salaries", async (PayrollDbContext db, IMapper mapper) =>
        {
            return await db.Salaries.Select(s => mapper.Map<SalaryResponse>(s)).ToListAsync();
        });

        app.MapGet("/api/payroll/salaries/{id}", async (Guid id, PayrollDbContext db, IMapper mapper) =>
        {
            var salary = await db.Salaries.FirstOrDefaultAsync(s => s.Id == id);
            return salary == null ? Results.NotFound() : Results.Ok(mapper.Map<SalaryResponse>(salary));
        });

        app.MapPost("/api/payroll/salaries", async (CreateSalaryRequest request, PayrollDbContext db, IMapper mapper) =>
        {
            var salary = mapper.Map<Salary>(request);
            salary.Id = Guid.NewGuid();
            salary.CreatedAt = DateTime.UtcNow;
            salary.UpdatedAt = DateTime.UtcNow;
            // TODO: Get CreatedBy and UpdatedBy from authenticated user
            salary.CreatedBy = Guid.Empty;
            salary.UpdatedBy = Guid.Empty;

            db.Salaries.Add(salary);
            await db.SaveChangesAsync();

            return Results.Created($"/api/payroll/salaries/{salary.Id}", mapper.Map<SalaryResponse>(salary));
        });

        app.MapPut("/api/payroll/salaries/{id}", async (Guid id, UpdateSalaryRequest request, PayrollDbContext db, IMapper mapper) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest();
            }

            var salary = await db.Salaries.FirstOrDefaultAsync(s => s.Id == id);
            if (salary == null)
            {
                return Results.NotFound();
            }

            mapper.Map(request, salary);
            salary.UpdatedAt = DateTime.UtcNow;
            // TODO: Get UpdatedBy from authenticated user
            salary.UpdatedBy = Guid.Empty;

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapDelete("/api/payroll/salaries/{id}", async (Guid id, PayrollDbContext db) =>
        {
            var salary = await db.Salaries.FirstOrDefaultAsync(s => s.Id == id);
            if (salary == null)
            {
                return Results.NotFound();
            }

            db.Salaries.Remove(salary);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        // Bonuses Endpoints
        app.MapGet("/api/payroll/bonuses", async (PayrollDbContext db, IMapper mapper) =>
        {
            return await db.Bonuses.Select(b => mapper.Map<BonusResponse>(b)).ToListAsync();
        });

        app.MapGet("/api/payroll/bonuses/{id}", async (Guid id, PayrollDbContext db, IMapper mapper) =>
        {
            var bonus = await db.Bonuses.FirstOrDefaultAsync(b => b.Id == id);
            return bonus == null ? Results.NotFound() : Results.Ok(mapper.Map<BonusResponse>(bonus));
        });

        app.MapPost("/api/payroll/bonuses", async (CreateBonusRequest request, PayrollDbContext db, IMapper mapper) =>
        {
            var bonus = mapper.Map<Bonus>(request);
            bonus.Id = Guid.NewGuid();
            bonus.CreatedAt = DateTime.UtcNow;
            bonus.UpdatedAt = DateTime.UtcNow;
            // TODO: Get CreatedBy and UpdatedBy from authenticated user
            bonus.CreatedBy = Guid.Empty;
            bonus.UpdatedBy = Guid.Empty;

            db.Bonuses.Add(bonus);
            await db.SaveChangesAsync();

            return Results.Created($"/api/payroll/bonuses/{bonus.Id}", mapper.Map<BonusResponse>(bonus));
        });

        app.MapPut("/api/payroll/bonuses/{id}", async (Guid id, UpdateBonusRequest request, PayrollDbContext db, IMapper mapper) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest();
            }

            var bonus = await db.Bonuses.FirstOrDefaultAsync(b => b.Id == id);
            if (bonus == null)
            {
                return Results.NotFound();
            }

            mapper.Map(request, bonus);
            bonus.UpdatedAt = DateTime.UtcNow;
            // TODO: Get UpdatedBy from authenticated user
            bonus.UpdatedBy = Guid.Empty;

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapDelete("/api/payroll/bonuses/{id}", async (Guid id, PayrollDbContext db) =>
        {
            var bonus = await db.Bonuses.FirstOrDefaultAsync(b => b.Id == id);
            if (bonus == null)
            {
                return Results.NotFound();
            }

            db.Bonuses.Remove(bonus);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
