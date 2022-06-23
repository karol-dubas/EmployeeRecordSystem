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
            CreateMap<ApplicationUser, EmployeeDeteilsDto>();
            CreateMap<ApplicationUser, EmployeeInGroupDto>();
            CreateMap<EditEmployeeRequest, ApplicationUser>();
            CreateMap<BalanceLog, BalanceLogDto>();
            CreateMap<UserBilling, UserBillingDto>();
        }
    }
}
