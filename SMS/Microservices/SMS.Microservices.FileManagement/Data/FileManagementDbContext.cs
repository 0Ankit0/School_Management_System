using Microsoft.EntityFrameworkCore;
using SMS.Microservices.FileManagement.Models;

namespace SMS.Microservices.FileManagement.Data;

public class FileManagementDbContext : DbContext
{
    public FileManagementDbContext(DbContextOptions<FileManagementDbContext> options) : base(options)
    {
    }

    public DbSet<FileStorage> FileStorages { get; set; }
}
