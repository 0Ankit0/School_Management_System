using Microsoft.EntityFrameworkCore;
using SMS.Microservices.Reporting.Models;

namespace SMS.Microservices.Reporting.Data;

public class ReportingDbContext : DbContext
{
    public ReportingDbContext(DbContextOptions<ReportingDbContext> options) : base(options)
    {
    }

    public DbSet<Report> Reports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
