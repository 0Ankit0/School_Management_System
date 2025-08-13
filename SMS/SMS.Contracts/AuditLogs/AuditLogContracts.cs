using System;

namespace SMS.Contracts.AuditLogs;

public class AuditLogResponse
{
    public Guid Id { get; set; }
    public Guid UserExternalId { get; set; }
    public string Action { get; set; }
    public string Entity { get; set; }
    public int EntityId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Details { get; set; }
}