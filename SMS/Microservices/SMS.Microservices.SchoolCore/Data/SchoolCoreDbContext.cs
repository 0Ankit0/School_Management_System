using Microsoft.EntityFrameworkCore;
using SMS.Microservices.SchoolCore.Models;

namespace SMS.Microservices.SchoolCore.Data;

public class SchoolCoreDbContext : DbContext
{
    public SchoolCoreDbContext(DbContextOptions<SchoolCoreDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<ParentGuardian> ParentGuardians { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<AcademicYear> AcademicYears { get; set; }
    public DbSet<Term> Terms { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Grade> Grades { get; set; }
}