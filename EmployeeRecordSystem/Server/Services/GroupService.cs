using AutoMapper;
using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Server.Exceptions;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.Shared.Requests;
using EmployeeRecordSystem.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using static EmployeeRecordSystem.Server.Installers.ServiceAttributes;

namespace EmployeeRecordSystem.Server.Services;

public interface IGroupService
{
    GroupDto Create(CreateGroupRequest request);
    List<GroupDto> GetAll(GroupQuery query);
    GroupDto Rename(Guid groupId, RenameGroupRequest request);
    void Delete(Guid groupId);
    void AssignEmployeeToGroup(Guid groupId, Guid employeeId);
    void RemoveEmployeeFromGroup(Guid employeeId);
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

    public List<GroupDto> GetAll(GroupQuery query)
    {
        var queryable = _dbContext.Groups
            .Include(g => g.Employees)
            .AsNoTracking();

        queryable = ApplyGetAllFilter(query, queryable);

        var groups = queryable.ToList();
        return _mapper.Map<List<GroupDto>>(groups);
    }

    public GroupDto Rename(Guid groupId, RenameGroupRequest request)
    {
        var group = _dbContext.Groups.SingleOrDefault(g => g.Id == groupId);

        if (group is null)
            throw new NotFoundException("Group");

        group.Name = request.Name;
        SaveChanges();
        
        return _mapper.Map<GroupDto>(group);
    }

    public void Delete(Guid groupId)
    {
        var group = _dbContext.Groups.SingleOrDefault(g => g.Id == groupId);

        if (group is null)
            throw new NotFoundException("Group");

        _dbContext.Groups.Remove(group);
        SaveChanges();
    }

    public void AssignEmployeeToGroup(Guid groupId, Guid employeeId)
    {
        var employee = _dbContext.Users.SingleOrDefault(u => u.Id == employeeId);

        if (employee is null)
            throw new NotFoundException("Employee");

        var group = _dbContext.Groups.SingleOrDefault(u => u.Id == groupId);

        if (group is null)
            throw new NotFoundException("Group");

        employee.GroupId = group.Id;
        SaveChanges();
    }

    public void RemoveEmployeeFromGroup(Guid employeeId)
    {
        var employee = _dbContext.Users.SingleOrDefault(u => u.Id == employeeId);

        if (employee is null)
            throw new NotFoundException("Employee");

        employee.GroupId = null;
        SaveChanges();
    }

    private IQueryable<Group> ApplyGetAllFilter(GroupQuery query, IQueryable<Group> queryable)
    {
        if (query.Id != default)
            queryable = queryable.Where(g => g.Id == query.Id);

        return queryable;
    }
}