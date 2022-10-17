using AutoMapper;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Server.Mappings
{
    public class WithdrawalRequestMappingProfile : Profile
    {
        public WithdrawalRequestMappingProfile()
        {
            CreateMap<WithdrawalRequest, CreatedWithdrawalRequestDto>()
                .ForMember(m => m.WithdrawalRequestStatus, c => c.MapFrom(s => s.WithdrawalRequestStatusTypeCode));

            CreateMap<WithdrawalRequest, WithdrawalRequestDto>()
                .ForMember(m => m.WithdrawalRequestStatus, c => c.MapFrom(s => s.WithdrawalRequestStatusTypeCode));

        }
    }
}
