namespace SMS.Microservices.NotificationMessaging.Models;

public class Notification
{
    public Guid Id { get; set; }
    public Guid ExternalId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; }
    public string Type { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
