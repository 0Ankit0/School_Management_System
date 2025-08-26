namespace SMS.Microservices.SchoolCore.Models;

public class Schedule
{
    public Guid Id { get; set; }
    public Guid ExternalId { get; set; }
    public Guid ClassId { get; set; }
    public Guid SubjectId { get; set; }
    public Guid TeacherId { get; set; }
    public Guid? TermId { get; set; }
    public string? Room { get; set; }
    public int? DayOfWeek { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid UpdatedBy { get; set; }
}
