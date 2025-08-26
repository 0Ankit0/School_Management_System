using Microsoft.EntityFrameworkCore;
using SMS.Microservices.SchoolCore.Data;
using SMS.Microservices.SchoolCore.Endpoints;
using SMS.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder.Services.AddDbContext<SchoolCoreDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SchoolCoreConnection")));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient<IEndpoint, StudentEndpoints>();
builder.Services.AddTransient<IEndpoint, CourseEndpoints>();
builder.Services.AddTransient<IEndpoint, ParentGuardianEndpoints>();
builder.Services.AddTransient<IEndpoint, AttendanceEndpoints>();
builder.Services.AddTransient<IEndpoint, AssignmentEndpoints>();
builder.Services.AddTransient<IEndpoint, AssignmentSubmissionEndpoints>();
builder.Services.AddTransient<IEndpoint, AuditLogEndpoints>();
builder.Services.AddTransient<IEndpoint, EnrollmentEndpoints>();
builder.Services.AddTransient<IEndpoint, TeacherEndpoints>();
builder.Services.AddTransient<IEndpoint, AcademicYearEndpoints>();
builder.Services.AddTransient<IEndpoint, TermEndpoints>();
builder.Services.AddTransient<IEndpoint, ClassEndpoints>();
builder.Services.AddTransient<IEndpoint, SubjectEndpoints>();
builder.Services.AddTransient<IEndpoint, ScheduleEndpoints>();
builder.Services.AddTransient<IEndpoint, GradeEndpoints>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapDefaultEndpoints();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<SchoolCoreDbContext>();
    context.Database.Migrate();
}

app.Run();
