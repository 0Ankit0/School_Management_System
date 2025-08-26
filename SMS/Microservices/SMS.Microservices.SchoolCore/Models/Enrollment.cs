namespace SMS.Microservices.SchoolCore.Models;

public class Enrollment
{
    public Guid Id { get; set; }
    public Guid ExternalId { get; set; }
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public string Grade { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid UpdatedBy { get; set; }
}
