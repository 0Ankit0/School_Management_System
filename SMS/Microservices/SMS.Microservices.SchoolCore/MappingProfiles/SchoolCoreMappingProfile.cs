using AutoMapper;
using SMS.Data.Models;
using SMS.Contracts.Students;
using SMS.Contracts.Courses;
using SMS.Contracts.ParentGuardians;
using SMS.Contracts.Attendances;
using SMS.Contracts.Assignments;
using SMS.Contracts.AssignmentSubmissions;
using SMS.Contracts.AuditLogs;

namespace SMS.Microservices.SchoolCore.MappingProfiles;

public class SchoolCoreMappingProfile : Profile
{
    public SchoolCoreMappingProfile()
    {
        CreateMap<Student, StudentResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId));
        CreateMap<Course, CourseResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId));
        CreateMap<ParentGuardian, ParentGuardianResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId));
        CreateMap<Attendance, AttendanceResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId));
        CreateMap<Assignment, AssignmentResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId));
        CreateMap<AssignmentSubmission, AssignmentSubmissionResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId));
        CreateMap<AuditLog, AuditLogResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId));
    }
}
