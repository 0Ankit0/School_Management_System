namespace SMS.Microservices.SchoolCore.Models;

public class Subject
{
    public Guid Id { get; set; }
    public Guid ExternalId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid UpdatedBy { get; set; }
}
