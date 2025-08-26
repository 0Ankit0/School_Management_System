namespace SMS.Microservices.SchoolCore.Models;

public class AssignmentSubmission
{
    public Guid Id { get; set; }
    public Guid ExternalId { get; set; }
    public Guid AssignmentId { get; set; }
    public Guid StudentId { get; set; }
    public Guid FileId { get; set; }
    public string FileName { get; set; }
    public DateTime SubmittedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid UpdatedBy { get; set; }
}
