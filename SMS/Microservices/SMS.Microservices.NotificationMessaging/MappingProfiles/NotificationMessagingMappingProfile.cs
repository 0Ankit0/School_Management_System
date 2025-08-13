using AutoMapper;
using SMS.Data.Models;
using SMS.Contracts.Messages;
using SMS.Contracts.Notifications;
using SMS.Contracts.Announcements;

namespace SMS.Microservices.NotificationMessaging.MappingProfiles;

public class NotificationMessagingMappingProfile : Profile
{
    public NotificationMessagingMappingProfile()
    {
        CreateMap<Message, MessageResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId));
        CreateMap<Notification, NotificationResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId));
        CreateMap<Announcement, AnnouncementResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId));
    }
}
