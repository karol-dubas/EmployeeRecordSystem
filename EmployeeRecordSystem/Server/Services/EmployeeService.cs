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
using static EmployeeRecordSystem.Server.Installers.ServiceAttributes;

namespace EmployeeRecordSystem.Server.Services;

public interface IEmployeeService
{
	EmployeeDeteilsDto GetDetails(Guid employeeId);
	List<EmployeeInGroupDto> GetAll(EmployeeQuery query);
	void Edit(Guid employeeId, EditEmployeeRequest request);
	void ChangeHourlyPay(Guid employeeId, ChangeEmployeeHourlyPayRequest request);
	void ChangeWorkTimes(ChangeEmployeesWorkTimeRequest request);
	List<BalanceLogDto> GetBalanceLog(Guid employeeId);
	void ConvertWorkTimeToBalance(ConvertTimeRequest request);
}

[ScopedRegistration]
public class EmployeeService : BaseService, IEmployeeService
{
	private readonly IAuthorizationService _authorizationService;
	private readonly UserManager<Employee> _userManager;

	public EmployeeService(
		ApplicationDbContext dbContext,
		IMapper mapper,
		UserManager<Employee> userManager,
		IAuthorizationService authorizationService)
		: base(dbContext, mapper)
	{
		_userManager = userManager;
		_authorizationService = authorizationService;
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
		bool isAuthorized = _authorizationService.IsUserOwnResource(employeeId) ||
		                    _authorizationService.IsAdmin();
		if (!isAuthorized)
			throw new ForbidException();

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

	public void Edit(Guid employeeId, EditEmployeeRequest request)
	{
		bool isAuthorized = _authorizationService.IsUserOwnResource(employeeId) ||
		                    _authorizationService.IsAdmin();
		if (!isAuthorized)
			throw new ForbidException();

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

		switch (request.WorkTimeOperation)
		{
			case WorkTimeOperations.Subtract:
				employees.ForEach(e => e.EmployeeBilling.TimeWorked -= request.WorkTime);
				break;
			case WorkTimeOperations.Add:
				employees.ForEach(e => e.EmployeeBilling.TimeWorked += request.WorkTime);
				break;
		}

		SaveChanges();
	}

	public List<BalanceLogDto> GetBalanceLog(Guid employeeId)
	{
		bool isAuthorized = _authorizationService.IsUserOwnResource(employeeId) ||
		                    _authorizationService.IsAdmin();
		if (!isAuthorized)
			throw new ForbidException();

		var employee = _dbContext.Users
			.Include(u => u.BalanceLogs)
			.SingleOrDefault(u => u.Id == employeeId);

		if (employee is null)
			throw new NotFoundException("Employee");

		return _mapper.Map<List<BalanceLogDto>>(employee.BalanceLogs);
	}

	public void ConvertWorkTimeToBalance(ConvertTimeRequest request)
	{
		var billings = _dbContext.EmployeeBillings
			.Include(b => b.Employee)
			.AsQueryable();

		if (request.GroupId != default)
			billings = billings.Where(b => b.Employee.GroupId == request.GroupId);

		ConvertBillingsToBalance(billings.ToList());
		SaveChanges();
	}

	private void ConvertBillingsToBalance(List<EmployeeBilling> employeeBillings)
	{
		foreach (var billing in employeeBillings)
		{
			var timeWorked = billing.TimeWorked;
			decimal balanceBefore = billing.Balance;

			decimal balanceToAdd = billing.HourlyPay * (decimal)timeWorked.TotalHours;
			billing.TimeWorked = TimeSpan.Zero;
			decimal balanceAfter = billing.Balance += balanceToAdd;

			CreateBalanceLog(balanceBefore, balanceAfter, billing.EmployeeId);
		}
	}

	private void CreateBalanceLog(decimal balanceBefore, decimal balanceAfter, Guid employeeId)
	{
		var balanceLog = new BalanceLog
		{
			BalanceBefore = balanceBefore,
			BalanceAfter = balanceAfter,
			EmployeeId = employeeId
		};

		_dbContext.BalanceLogs.Add(balanceLog);
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

	private string GetEmployeeRole(Employee employee)
	{
		return _userManager.GetRolesAsync(employee).Result.Single();
	}
}
