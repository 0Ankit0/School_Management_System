namespace SMS.Microservices.SchoolCore.Models;

public class Course
{
    public Guid Id { get; set; }
    public Guid ExternalId { get; set; }
    public string CourseCode { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Credits { get; set; }
    public Guid TeacherId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid UpdatedBy { get; set; }
}
