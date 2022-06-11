using AutoMapper;
using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Server.Queries;
using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Requests;
using EmployeeRecordSystem.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmployeeRecordSystem.Server.Installers.ServiceAttributes;

namespace EmployeeRecordSystem.Server.Services
{
    public interface IEmployeeService
    {
        EmployeeDeteilsDto GetDetails(Guid employeeId);
        List<EmployeeInGroupDto> GetAll(EmployeeQuery query);
        void Edit(Guid employeeId, EditEmployeeRequest request);
        void ChangeHourlyPay(Guid employeeId, ChangeEmployeeHourlyPayRequest request);
        void ChangeWorkTimes(ChangeEmployeesWorkTimeRequest request);
    }

    [ScopedRegistration]
    public class EmployeeService : BaseService, IEmployeeService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeService(ApplicationDbContext dbContext, IMapper mapper,
            UserManager<ApplicationUser> userManager)
            : base(dbContext, mapper)
        {
            _userManager = userManager;
        }

        public List<EmployeeInGroupDto> GetAll(EmployeeQuery query)
        {
            var queryable = _dbContext.Users.AsNoTracking();
            queryable = ApplyGetAllFilter(query, queryable);

            var employees = queryable.ToList();
            employees.ForEach(u => u.Role = GetUserRole(u));

            return _mapper.Map<List<EmployeeInGroupDto>>(employees);
        }

        public EmployeeDeteilsDto GetDetails(Guid employeeId)
        {
            var user = _dbContext.Users
                .AsNoTracking()
                .Include(u => u.Group)
                .Include(u => u.UserBilling)
                .SingleOrDefault(u => u.Id == employeeId);

            user.Role = GetUserRole(user);

            // TODO: user null check

            return _mapper.Map<EmployeeDeteilsDto>(user);
        }

        private string GetUserRole(ApplicationUser user)
        {
            return _userManager.GetRolesAsync(user).Result.Single();
        }

        public void Edit(Guid employeeId, EditEmployeeRequest request)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Id == employeeId);

            // TODO: null check

            _mapper.Map(request, user);
            SaveChanges();
        }

        public void ChangeHourlyPay(Guid employeeId, ChangeEmployeeHourlyPayRequest request)
        {
            var employee = _dbContext.Users
                .Include(u => u.UserBilling)
                .SingleOrDefault(u => u.Id == employeeId);

            // TODO: null check

            employee.UserBilling.HourlyPay = request.HourlyPay;
            SaveChanges();
        }

        public void ChangeWorkTimes(ChangeEmployeesWorkTimeRequest request)
        {
            var employees = _dbContext.Users
                .Include(u => u.UserBilling)
                .Where(u => request.EmployeeIds.Contains(u.Id))
                .ToList();

            if (!employees.Any())
                return;

            if (request.WorkTimeOperation is WorkTimeOperations.Subtract)
                employees.ForEach(e => e.UserBilling.TimeWorked -= request.WorkTime);

            if (request.WorkTimeOperation is WorkTimeOperations.Add)
                employees.ForEach(e => e.UserBilling.TimeWorked += request.WorkTime);

            SaveChanges();
        }

        private static IQueryable<ApplicationUser> ApplyGetAllFilter(EmployeeQuery query, IQueryable<ApplicationUser> queryable)
        {
            if (query.GroupId != default)
            {
                // TODO: group null check

                queryable = queryable
                    .Include(u => u.Group)
                    .Where(u => u.GroupId == query.GroupId);
            }

            return queryable;
        }
    }
}
