using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Shared.Requests;
using EmployeeRecordSystem.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmployeeRecordSystem.Server.Installers.ServiceAttributes;

namespace EmployeeRecordSystem.Server.Services
{
    public interface IGroupService
    {
        GroupDto Create(CreateGroupRequest request);
        GroupDto Get(Guid groupId);
        List<GroupDto> GetAll();
        GroupDto Rename(Guid groupId, RenameGroupRequest request);
    }

    [ScopedRegistration]
    public class GroupService : BaseService, IGroupService
    {
        public GroupService(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

        public GroupDto Create(CreateGroupRequest request)
        {
            var group = _mapper.Map<Group>(request);
            _dbContext.Groups.Add(group);
            SaveChanges();
            return _mapper.Map<GroupDto>(group);
        }

        public List<GroupDto> GetAll()
        {
            var groups = _dbContext.Groups
                .AsNoTracking()
                .ToList();

            return _mapper.Map<List<GroupDto>>(groups);
        }

        public GroupDto Get(Guid groupId)
        {
            var group = _dbContext.Groups
                .AsNoTracking()
                .SingleOrDefault(g => g.Id == groupId);

            // TODO: null check

            return _mapper.Map<GroupDto>(group);
        }

        public GroupDto Rename(Guid groupId, RenameGroupRequest request)
        {
            var group = _dbContext.Groups.SingleOrDefault(g => g.Id == groupId);

            // TODO: null check

            group.Name = request.Name;
            SaveChanges();
            return _mapper.Map<GroupDto>(group);
        }
    }
}
