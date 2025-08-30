using Microsoft.EntityFrameworkCore;
using SMS.Microservices.Payroll.Models;

namespace SMS.Microservices.Payroll.Data;

public class PayrollDbContext : DbContext
{
    public PayrollDbContext(DbContextOptions<PayrollDbContext> options) : base(options)
    {
    }

    public DbSet<Salary> Salaries { get; set; }
    public DbSet<Bonus> Bonuses { get; set; }
    public DbSet<Deduction> Deductions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
