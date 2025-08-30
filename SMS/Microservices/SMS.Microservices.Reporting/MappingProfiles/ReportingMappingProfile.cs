using AutoMapper;
using SMS.Contracts.Reporting;
using SMS.Microservices.Reporting.Models;

namespace SMS.Microservices.Reporting.MappingProfiles;

public class ReportingMappingProfile : Profile
{
    public ReportingMappingProfile()
    {
        CreateMap<Report, ReportResponse>();
        CreateMap<CreateReportRequest, Report>();
        CreateMap<UpdateReportRequest, Report>();
    }
}
