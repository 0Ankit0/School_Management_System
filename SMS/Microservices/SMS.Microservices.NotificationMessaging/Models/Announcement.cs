namespace SMS.Microservices.NotificationMessaging.Models;

public class Announcement
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string TargetAudience { get; set; }
    public DateTime CreatedAt { get; set; }
}
