namespace SMS.Microservices.SchoolCore.Models;

public class Attendance
{
    public Guid Id { get; set; }
    public Guid ExternalId { get; set; }
    public Guid StudentId { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
    public string Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid UpdatedBy { get; set; }
}
