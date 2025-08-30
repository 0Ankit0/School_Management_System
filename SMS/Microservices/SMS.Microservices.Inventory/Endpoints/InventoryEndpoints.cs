using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMS.Contracts.Inventory;
using SMS.Microservices.Inventory.Data;
using SMS.Microservices.Inventory.Models;
using SMS.ServiceDefaults;

namespace SMS.Microservices.Inventory.Endpoints;

public class InventoryEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/inventory/items", async (InventoryDbContext db, IMapper mapper) =>
        {
            return await db.InventoryItems.Select(item => mapper.Map<InventoryItemResponse>(item)).ToListAsync();
        });

        app.MapGet("/api/inventory/items/{id}", async (Guid id, InventoryDbContext db, IMapper mapper) =>
        {
            var item = await db.InventoryItems.FirstOrDefaultAsync(item => item.Id == id);
            return item == null ? Results.NotFound() : Results.Ok(mapper.Map<InventoryItemResponse>(item));
        });

        app.MapPost("/api/inventory/items", async (CreateInventoryItemRequest request, InventoryDbContext db, IMapper mapper) =>
        {
            var item = mapper.Map<InventoryItem>(request);
            item.Id = Guid.NewGuid();
            item.CreatedAt = DateTime.UtcNow;
            item.UpdatedAt = DateTime.UtcNow;
            // TODO: Get CreatedBy and UpdatedBy from authenticated user
            item.CreatedBy = Guid.Empty;
            item.UpdatedBy = Guid.Empty;

            db.InventoryItems.Add(item);
            await db.SaveChangesAsync();

            return Results.Created($"/api/inventory/items/{item.Id}", mapper.Map<InventoryItemResponse>(item));
        });

        app.MapPut("/api/inventory/items/{id}", async (Guid id, UpdateInventoryItemRequest request, InventoryDbContext db, IMapper mapper) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest();
            }

            var item = await db.InventoryItems.FirstOrDefaultAsync(item => item.Id == id);
            if (item == null)
            {
                return Results.NotFound();
            }

            mapper.Map(request, item);
            item.UpdatedAt = DateTime.UtcNow;
            // TODO: Get UpdatedBy from authenticated user
            item.UpdatedBy = Guid.Empty;

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapDelete("/api/inventory/items/{id}", async (Guid id, InventoryDbContext db) =>
        {
            var item = await db.InventoryItems.FirstOrDefaultAsync(item => item.Id == id);
            if (item == null)
            {
                return Results.NotFound();
            }

            db.InventoryItems.Remove(item);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}

