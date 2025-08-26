namespace SMS.Microservices.SchoolCore.Models;

public class Class
{
    public Guid Id { get; set; }
    public Guid ExternalId { get; set; }
    public Guid AcademicYearId { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
    public Guid? HomeroomTeacherId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid UpdatedBy { get; set; }
}
