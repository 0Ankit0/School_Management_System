namespace SMS.Microservices.SchoolCore.Models;

public class Grade
{
    public Guid Id { get; set; }
    public Guid ExternalId { get; set; }
    public Guid? EnrollmentId { get; set; }
    public Guid? AssignmentId { get; set; }
    public Guid StudentId { get; set; }
    public Guid? CourseId { get; set; }
    public Guid? TermId { get; set; }
    public decimal Score { get; set; }
    public decimal MaxScore { get; set; }
    public string? Letter { get; set; }
    public decimal Weight { get; set; }
    public string? Comment { get; set; }
    public DateTime RecordedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid UpdatedBy { get; set; }
}
