using AutoMapper;
using SMS.Contracts.FileStorage;
using SMS.Microservices.FileManagement.Models;

namespace SMS.Microservices.FileManagement.MappingProfiles;

public class FileManagementMappingProfile : Profile
{
    public FileManagementMappingProfile()
    {
        CreateMap<FileStorage, FileStorageResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UploadedByUserExternalId, opt => opt.MapFrom(src => src.UploadedByUserId));
    }
}