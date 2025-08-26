using Microsoft.EntityFrameworkCore;
using SMS.Microservices.NotificationMessaging.Models;

namespace SMS.Microservices.NotificationMessaging.Data;

public class NotificationMessagingDbContext : DbContext
{
    public NotificationMessagingDbContext(DbContextOptions<NotificationMessagingDbContext> options) : base(options)
    {
    }

    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Notification> Notifications { get; set; }
}