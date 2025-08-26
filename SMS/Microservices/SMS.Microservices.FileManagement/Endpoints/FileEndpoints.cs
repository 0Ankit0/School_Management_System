using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SMS.Contracts.FileStorage;
using SMS.Microservices.FileManagement.Data;
using SMS.Microservices.FileManagement.Models;
using SMS.ServiceDefaults;

namespace SMS.Microservices.FileManagement.Endpoints;

public class FileEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/files/upload", async (IFormFile file, FileManagementDbContext dbContext, BlobServiceClient blobServiceClient, IConfiguration configuration) =>
        {
            var containerName = configuration["AzureStorage:ContainerName"];
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(file.FileName);
            await blobClient.UploadAsync(file.OpenReadStream(), true);

            var fileStorage = new FileStorage
            {
                Id = Guid.NewGuid(),
                FileName = file.FileName,
                FilePath = blobClient.Uri.ToString(),
                UploadedByUserId = Guid.NewGuid(), // Replace with actual user ID
                UploadedAt = DateTime.UtcNow,
                ContentType = file.ContentType,
                Length = file.Length,
                Version = 1
            };

            await dbContext.FileStorages.AddAsync(fileStorage);
            await dbContext.SaveChangesAsync();

            return Results.Created($"/api/files/{fileStorage.Id}", null);
        });

        app.MapGet("/api/files/download/{id}", async (Guid id, FileManagementDbContext dbContext, BlobServiceClient blobServiceClient, IConfiguration configuration) =>
        {
            var fileStorage = await dbContext.FileStorages.FindAsync(id);
            if (fileStorage == null)
            {
                return Results.NotFound();
            }

            var containerName = configuration["AzureStorage:ContainerName"];
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileStorage.FileName);

            var stream = await blobClient.OpenReadAsync();
            return Results.File(stream, fileStorage.ContentType, fileStorage.FileName);
        });
    }
}
