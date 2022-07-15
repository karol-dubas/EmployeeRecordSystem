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
    public class GroupMappingProfile : Profile
    {
        public GroupMappingProfile()
        {
            CreateMap<CreateGroupRequest, Group>();
            CreateMap<Group, GroupDto>()
                .ForMember(m => m.IsEmpty, c => c.MapFrom(s => s.Employees.Count == 0));
        }
    }
}
