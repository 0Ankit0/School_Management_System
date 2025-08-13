using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SMS.Microservices.User.Models;

namespace SMS.Microservices.User.Data;

public class UserDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public override int SaveChanges()
    {
        AddTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AddTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void AddTimestamps()
    {
        var entities = ChangeTracker.Entries()
            .Where(x => x.Entity is not null && (x.State == EntityState.Added || x.State == EntityState.Modified));

        // TODO: Replace with actual user ID from authentication context
        var currentUserId = "1"; // Changed to string for IdentityUser

        foreach (var entity in entities)
        {
            if (entity.State == EntityState.Added)
            {
                if (entity.Entity.GetType().GetProperty("CreatedAt") != null)
                {
                    entity.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                }
                if (entity.Entity.GetType().GetProperty("CreatedBy") != null)
                {
                    entity.Property("CreatedBy").CurrentValue = currentUserId;
                }
            }

            if (entity.Entity.GetType().GetProperty("UpdatedAt") != null)
            {
                entity.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }
            if (entity.Entity.GetType().GetProperty("UpdatedBy") != null)
            {
                entity.Property("UpdatedBy").CurrentValue = currentUserId;
            }
        }
    }
}