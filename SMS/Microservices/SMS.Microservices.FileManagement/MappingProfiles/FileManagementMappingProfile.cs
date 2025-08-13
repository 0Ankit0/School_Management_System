using AutoMapper;
using SMS.Data.Models;
using SMS.Contracts.FileStorage;

namespace SMS.Microservices.FileManagement.MappingProfiles;

public class FileManagementMappingProfile : Profile
{
    public FileManagementMappingProfile()
    {
        CreateMap<FileStorage, FileStorageResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId));
    }
}
