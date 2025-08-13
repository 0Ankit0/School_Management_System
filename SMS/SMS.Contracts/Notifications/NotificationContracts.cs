using System;

namespace SMS.Contracts.Notifications;

public class NotificationResponse
{
    public Guid Id { get; set; }
    public Guid UserExternalId { get; set; }
    public string Content { get; set; }
    public string Type { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}