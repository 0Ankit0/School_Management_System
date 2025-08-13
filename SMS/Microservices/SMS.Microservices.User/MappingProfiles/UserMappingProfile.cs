using AutoMapper;
using SMS.Data.Models;
using SMS.Contracts.Users;

namespace SMS.Microservices.User.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId));
    }
}
