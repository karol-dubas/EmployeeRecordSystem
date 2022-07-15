using AutoMapper;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Shared.Requests;
using EmployeeRecordSystem.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Server.Mappings
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            CreateMap<Employee, EmployeeDeteilsDto>();
            CreateMap<Employee, EmployeeInGroupDto>()
                .ForMember(m => m.HourlyPay, c => c.MapFrom(s => s.EmployeeBilling.HourlyPay))
                .ForMember(m => m.TimeWorked, c => c.MapFrom(s => s.EmployeeBilling.TimeWorked));
            CreateMap<EditEmployeeRequest, Employee>();
            CreateMap<BalanceLog, BalanceLogDto>();
            CreateMap<EmployeeBilling, EmployeeBillingDto>();
        }
    }
}
