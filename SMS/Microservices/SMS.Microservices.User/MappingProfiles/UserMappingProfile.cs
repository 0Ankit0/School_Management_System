using AutoMapper;
using SMS.Contracts.Users;
using SMS.Microservices.User.Models;

namespace SMS.Microservices.User.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<ApplicationUser, UserResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
    }
}
