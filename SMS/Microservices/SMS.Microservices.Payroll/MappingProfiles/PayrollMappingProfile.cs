using AutoMapper;
using SMS.Contracts.Payroll;
using SMS.Microservices.Payroll.Models;

namespace SMS.Microservices.Payroll.MappingProfiles;

public class PayrollMappingProfile : Profile
{
    public PayrollMappingProfile()
    {
        CreateMap<Salary, SalaryResponse>();
        CreateMap<Bonus, BonusResponse>();
        CreateMap<Deduction, DeductionResponse>();
        CreateMap<CreateSalaryRequest, Salary>();
        CreateMap<UpdateSalaryRequest, Salary>();
        CreateMap<CreateBonusRequest, Bonus>();
        CreateMap<UpdateBonusRequest, Bonus>();
    }
}
