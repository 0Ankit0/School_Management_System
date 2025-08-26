using Microsoft.EntityFrameworkCore;
using SMS.Microservices.User.Models;

namespace SMS.Microservices.User.Data;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = Guid.NewGuid(), Name = "Administrator" },
            new Role { Id = Guid.NewGuid(), Name = "Teacher" },
            new Role { Id = Guid.NewGuid(), Name = "Student" },
            new Role { Id = Guid.NewGuid(), Name = "Parent" }
        );
    }
}
