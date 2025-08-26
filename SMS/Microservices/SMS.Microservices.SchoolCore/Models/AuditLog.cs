namespace SMS.Microservices.SchoolCore.Models;

public class AuditLog
{
    public Guid Id { get; set; }
    public Guid ExternalId { get; set; }
    public Guid UserId { get; set; }
    public string Action { get; set; }
    public string Entity { get; set; }
    public int EntityId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Details { get; set; }
}
