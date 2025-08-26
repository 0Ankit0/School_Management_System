using AutoMapper;
using SMS.Contracts.Announcements;
using SMS.Contracts.Messages;
using SMS.Contracts.Notifications;
using SMS.Microservices.NotificationMessaging.Models;

namespace SMS.Microservices.NotificationMessaging.MappingProfiles;

public class NotificationMessagingMappingProfile : Profile
{
    public NotificationMessagingMappingProfile()
    {
        CreateMap<Announcement, AnnouncementResponse>();
        CreateMap<Message, MessageResponse>();
        CreateMap<Notification, NotificationResponse>();
    }
}