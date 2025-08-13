using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.Microservices.FileManagement.Data;
using SMS.Microservices.FileManagement.Models;
using SMS.Contracts.FileStorage;
using AutoMapper;
using Dapper;
using System.Data;
using Npgsql;
using SMS.ServiceDefaults;
using System.Text.Json;

namespace SMS.Microservices.FileManagement.Endpoints;

public class FileEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/files", GetFiles)
            .WithName("GetFiles");

        app.MapGet("/api/files/{id}", GetFile)
            .WithName("GetFile");

        app.MapPost("/api/files", PostFile)
            .WithName("UploadFile");

        app.MapPut("/api/files/{id}/metadata", UpdateFileMetadata)
            .WithName("UpdateFileMetadata");

        app.MapPut("/api/files/{id}/access", UpdateFileAccess)
            .WithName("UpdateFileAccess");

        app.MapGet("/api/files/user/{userExternalId}", GetFilesUploadedByUser)
            .WithName("GetFilesUploadedByUser");
    }

    public static async Task<IResult> GetFiles(
        FileManagementDbContext context,
        IMapper mapper)
    {
        var files = await context.FileStorage.ToListAsync();
        return Results.Ok(mapper.Map<IEnumerable<FileStorageResponse>>(files));
    }

    public static async Task<IResult> GetFile(
        Guid id,
        FileManagementDbContext context,
        IMapper mapper)
    {
        var file = await context.FileStorage.FirstOrDefaultAsync(f => f.ExternalId == id);

        if (file == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(mapper.Map<FileStorageResponse>(file));
    }

    public static async Task<IResult> PostFile(
        FileUploadRequest request,
        FileManagementDbContext context,
        IMapper mapper)
    {
        // In a real scenario, you would handle the actual file upload here
        // and save it to cloud storage (Azure Blob, AWS S3) or local disk.
        // For now, we just save metadata.

        var fileStorage = mapper.Map<FileStorage>(request);
        fileStorage.FilePath = $"/uploads/{Guid.NewGuid()}_{request.FileName}"; // Example path with unique name
        fileStorage.UploadedAt = DateTime.UtcNow;
        fileStorage.UploadedBy = 1; // TODO: Get actual user ID from authentication context
        fileStorage.Metadata = JsonSerializer.Serialize(request.Metadata); // Serialize metadata

        context.FileStorage.Add(fileStorage);
        await context.SaveChangesAsync();

        return Results.CreatedAtRoute("GetFile", new { id = fileStorage.ExternalId }, mapper.Map<FileStorageResponse>(fileStorage));
    }

    public static async Task<IResult> UpdateFileMetadata(
        Guid id,
        UpdateFileMetadataRequest request,
        FileManagementDbContext context,
        IMapper mapper)
    {
        var file = await context.FileStorage.FirstOrDefaultAsync(f => f.ExternalId == id);
        if (file == null)
        {
            return Results.NotFound();
        }

        file.Metadata = JsonSerializer.Serialize(request.Metadata);
        file.UpdatedAt = DateTime.UtcNow;
        file.UpdatedBy = 1; // TODO: Get actual user ID

        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    public static async Task<IResult> UpdateFileAccess(
        Guid id,
        UpdateFileAccessRequest request,
        FileManagementDbContext context,
        IMapper mapper)
    {
        var file = await context.FileStorage.FirstOrDefaultAsync(f => f.ExternalId == id);
        if (file == null)
        {
            return Results.NotFound();
        }

        // TODO: Implement actual access control logic (e.g., store permissions in a separate table or within metadata)
        // For now, just a placeholder.
        file.Metadata = JsonSerializer.Serialize(new Dictionary<string, string>(JsonSerializer.Deserialize<Dictionary<string, string>>(file.Metadata))
        {
            {"AllowedUserIds", JsonSerializer.Serialize(request.AllowedUserIds)},
            {"AllowedRoleIds", JsonSerializer.Serialize(request.AllowedRoleIds)}
        });
        file.UpdatedAt = DateTime.UtcNow;
        file.UpdatedBy = 1; // TODO: Get actual user ID

        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    public static async Task<IResult> GetFilesUploadedByUser(
        Guid userExternalId,
        IConfiguration configuration)
    {
        using IDbConnection dbConnection = new NpgsqlConnection(configuration.GetConnectionString("FileManagementDbConnection"));
        // This query assumes a 'Users' table exists and has an 'ExternalId' column.
        // In a microservices architecture, you would typically call the User Service to get the internal UserId from the ExternalId.
        var sql = "SELECT * FROM \"FileStorage\" WHERE \"UploadedBy\" = (SELECT \"Id\" FROM \"Users\" WHERE \"ExternalId\" = @UserExternalId)";
        var files = await dbConnection.QueryAsync<FileStorage>(sql, new { UserExternalId = userExternalId });
        return Results.Ok(files); // Assuming FileStorage is already the DTO or will be mapped later
    }
}