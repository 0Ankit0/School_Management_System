namespace SMS.Microservices.SchoolCore.Models;

public class Assignment
{
    public Guid Id { get; set; }
    public Guid ExternalId { get; set; }
    public Guid CourseId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid UpdatedBy { get; set; }
}
