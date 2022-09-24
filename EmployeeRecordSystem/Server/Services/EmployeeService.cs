using AutoMapper;
using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Server.Exceptions;
using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Queries;
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
        List<BalanceLogDto> GetBalanceLog(Guid employeeId);
        void ConvertWorkTimeToBalance();
    }

    [ScopedRegistration]
    public class EmployeeService : BaseService, IEmployeeService
    {
        private readonly UserManager<Employee> _userManager;

        public EmployeeService(ApplicationDbContext dbContext, IMapper mapper,
            UserManager<Employee> userManager)
            : base(dbContext, mapper)
        {
            _userManager = userManager;
        }

        public List<EmployeeInGroupDto> GetAll(EmployeeQuery query)
        {
            var queryable = _dbContext.Users
                .Include(e => e.Group)
                .Include(e => e.EmployeeBilling)
                .AsNoTracking();

            queryable = ApplyGetAllFilter(query, queryable);

            var employees = queryable.ToList();
            employees.ForEach(u => u.Role = GetEmployeeRole(u));

            return _mapper.Map<List<EmployeeInGroupDto>>(employees);
        }

        public EmployeeDeteilsDto GetDetails(Guid employeeId)
        {
            var employee = _dbContext.Users
                .AsNoTracking()
                .Include(u => u.Group)
                .Include(u => u.EmployeeBilling)
                .SingleOrDefault(u => u.Id == employeeId);

            if (employee is null)
                throw new NotFoundException("Employee");

            employee.Role = GetEmployeeRole(employee);
         
            return _mapper.Map<EmployeeDeteilsDto>(employee);
        }

        private string GetEmployeeRole(Employee employee)
        {
            return _userManager.GetRolesAsync(employee).Result.Single();
        }

        public void Edit(Guid employeeId, EditEmployeeRequest request)
        {
            var employee = _dbContext.Users.SingleOrDefault(u => u.Id == employeeId);

            if (employee is null)
                throw new NotFoundException("Employee");

            _mapper.Map(request, employee);
            SaveChanges();
        }

        public void ChangeHourlyPay(Guid employeeId, ChangeEmployeeHourlyPayRequest request)
        {
            var employee = _dbContext.Users
                .Include(u => u.EmployeeBilling)
                .SingleOrDefault(u => u.Id == employeeId);

            if (employee is null)
                throw new NotFoundException("Employee");

            employee.EmployeeBilling.HourlyPay = request.HourlyPay;
            SaveChanges();
        }

        public void ChangeWorkTimes(ChangeEmployeesWorkTimeRequest request)
        {
            var employees = _dbContext.Users
                .Include(u => u.EmployeeBilling)
                .Where(u => request.EmployeeIds.Contains(u.Id))
                .ToList();

            if (!employees.Any())
                return;

            if (request.WorkTimeOperation is WorkTimeOperations.Subtract)
                employees.ForEach(e => e.EmployeeBilling.TimeWorked -= request.WorkTime);

            if (request.WorkTimeOperation is WorkTimeOperations.Add)
                employees.ForEach(e => e.EmployeeBilling.TimeWorked += request.WorkTime);

            SaveChanges();
        }

        private IQueryable<Employee> ApplyGetAllFilter(EmployeeQuery query, IQueryable<Employee> queryable)
        {
            if (query.WithoutGroup)
            {
                queryable = queryable
                    .Include(u => u.Group)
                    .Where(u => u.GroupId == null);
            }

            if (query.GroupId != default)
            {
                bool groupExists = _dbContext.Groups.Any(g => g.Id == query.GroupId);
                if (!groupExists)
                    throw new NotFoundException("Group");

                queryable = queryable
                    .Include(u => u.Group)
                    .Where(u => u.GroupId == query.GroupId);
            }

            return queryable;
        }

        public List<BalanceLogDto> GetBalanceLog(Guid employeeId)
        {
            var employee = _dbContext.Users
                .Include(u => u.BalanceLogs)
                .SingleOrDefault(u => u.Id == employeeId);

            if (employee is null)
                throw new NotFoundException("Employee");

            return _mapper.Map<List<BalanceLogDto>>(employee.BalanceLogs);
        }

        public void ConvertWorkTimeToBalance()
        {
            var employeeBillings = _dbContext.EmployeeBillings.ToList();

            foreach (var billing in employeeBillings)
            {
                decimal hourlyPay = billing.HourlyPay;
                TimeSpan timeWorked = billing.TimeWorked;
                decimal balanceBefore = billing.Balance;

                decimal balanceToAdd = hourlyPay * (decimal)timeWorked.TotalHours;
                billing.TimeWorked = TimeSpan.Zero;
                decimal balanceAfter = billing.Balance += balanceToAdd;

                var balanceLog = new BalanceLog()
                {
                    BalanceBefore = balanceBefore,
                    BalanceAfter = balanceAfter,
                    EmployeeId = billing.EmployeeId
                };

                _dbContext.BalanceLogs.Add(balanceLog);
            }

            SaveChanges();
        }
    }
}
