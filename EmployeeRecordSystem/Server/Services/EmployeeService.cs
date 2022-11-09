using System.Linq.Expressions;
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
using MudBlazor;
using MudBlazor.Extensions;
using static EmployeeRecordSystem.Server.Installers.Helpers.ServiceAttributes;

namespace EmployeeRecordSystem.Server.Services;

public interface IEmployeeService
{
	EmployeeDeteilsDto GetDetails(Guid employeeId);
	List<EmployeeInGroupDto> GetAll(EmployeeQuery query);
	void Edit(Guid employeeId, EditEmployeeRequest request);
	void ChangeHourlyPay(Guid employeeId, ChangeEmployeeHourlyPayRequest request);
	void ChangeWorkTimes(ChangeEmployeesWorkTimeRequest request);
	PagedContent<BalanceLogDto> GetBalanceLogs(Guid employeeId, BalanceLogQuery query);
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
		var queryable = DbContext.Users
			.Include(e => e.Group)
			.Include(e => e.EmployeeBilling)
			.AsNoTracking();

		queryable = ApplyGetAllFilter(query, queryable);

		if (!_authorizationService.IsAdmin())
			queryable = LimitData(queryable);

		var employees = queryable.ToList();

		foreach (var u in employees)
			u.Role = GetEmployeeRole(u);

		var map = Mapper.Map<List<EmployeeInGroupDto>>(employees);
		return map;
	}

	public EmployeeDeteilsDto GetDetails(Guid employeeId)
	{
		bool isAuthorized = _authorizationService.IsUserOwnResource(employeeId) ||
		                    _authorizationService.IsAdmin();
		if (!isAuthorized)
			throw new ForbidException();

		var employee = DbContext.Users
			.AsNoTracking()
			.Include(u => u.Group)
			.Include(u => u.EmployeeBilling)
			.SingleOrDefault(u => u.Id == employeeId);

		if (employee is null)
			throw new NotFoundException(nameof(employeeId), "Employee");

		employee.Role = GetEmployeeRole(employee);

		return Mapper.Map<EmployeeDeteilsDto>(employee);
	}

	public void Edit(Guid employeeId, EditEmployeeRequest request)
	{
		bool isAuthorized = _authorizationService.IsUserOwnResource(employeeId) ||
		                    _authorizationService.IsAdmin();
		if (!isAuthorized)
			throw new ForbidException();

		var employee = DbContext.Users.SingleOrDefault(u => u.Id == employeeId);

		if (employee is null)
			throw new NotFoundException(nameof(employeeId), "Employee");

		Mapper.Map(request, employee);
		SaveChanges();
	}

	public void ChangeHourlyPay(Guid employeeId, ChangeEmployeeHourlyPayRequest request)
	{
		var employee = DbContext.Users
			.Include(u => u.EmployeeBilling)
			.SingleOrDefault(u => u.Id == employeeId);

		if (employee is null)
			throw new NotFoundException(nameof(employeeId), "Employee");

		employee.EmployeeBilling.HourlyPay = request.HourlyPay;
		SaveChanges();
	}

	public void ChangeWorkTimes(ChangeEmployeesWorkTimeRequest request)
	{
		var queryable = DbContext.Users
			.Include(u => u.EmployeeBilling)
			.Include(u => u.Group)
			.Where(u => request.EmployeeIds.Contains(u.Id));

		if (_authorizationService.IsSupervisor())
			AuthorizeSupervisor(queryable);

		var employees = queryable.ToList();

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

	public PagedContent<BalanceLogDto> GetBalanceLogs(Guid employeeId, BalanceLogQuery query)
	{
		bool isAuthorized = _authorizationService.IsUserOwnResource(employeeId) ||
		                    _authorizationService.IsAdmin();
		if (!isAuthorized)
			throw new ForbidException();

		bool employeeExists = DbContext.Users.Any(e => e.Id == employeeId);
		if (!employeeExists)
			throw new NotFoundException(nameof(employeeId), "Employee");

		var queryable = DbContext.BalanceLogs
			.Include(u => u.Employee)
			.Where(bl => bl.EmployeeId == employeeId);

		(queryable, int totalItemsCount) = ApplyBalanceLogsQuery(query, queryable);

		var balanceLogs = queryable.ToList();

		var dtos = Mapper.Map<List<BalanceLogDto>>(balanceLogs);
		return new PagedContent<BalanceLogDto>(dtos, totalItemsCount);
	}

	public void ConvertWorkTimeToBalance(ConvertTimeRequest request)
	{
		var billings = DbContext.EmployeeBillings
			.Include(b => b.Employee)
			.AsQueryable();

		if (request.GroupId != default)
			billings = billings.Where(b => b.Employee.GroupId == request.GroupId);

		ConvertBillingsToBalance(billings.ToList());
		SaveChanges();
	}

	private (IQueryable<BalanceLog>, int totalItemsCount) ApplyBalanceLogsQuery(
		BalanceLogQuery query,
		IQueryable<BalanceLog> queryable)
	{
		if (!string.IsNullOrEmpty(query.SortBy))
		{
			var columnsSelector = new Dictionary<string, Expression<Func<BalanceLog, object>>>
			{
				{ nameof(BalanceLog.CreatedAt), bl => bl.CreatedAt },
				{ nameof(BalanceLog.BalanceAfter), bl => bl.BalanceAfter > bl.BalanceBefore }
			};

			var selectedColumn = columnsSelector[query.SortBy];

			if (query.SortDirection == SortDirection.Ascending.ToDescriptionString())
				queryable = queryable.OrderBy(selectedColumn);
			else if (query.SortDirection == SortDirection.Descending.ToDescriptionString())
				queryable = queryable.OrderByDescending(selectedColumn);
		}

		int totalItemsCount = queryable.Count();

		queryable = queryable
			.Skip(query.PageSize * (query.PageNumber - 1))
			.Take(query.PageSize);

		return (queryable, totalItemsCount);
	}

	private static IQueryable<Employee> LimitData(IQueryable<Employee> queryable)
	{
		return queryable.Select(e => new Employee
		{
			Id = e.Id,
			FirstName = e.FirstName,
			LastName = e.LastName,
			Group = e.Group
		});
	}

	private void AuthorizeSupervisor(IQueryable<Employee> queryable)
	{
		var supervisor = DbContext.Users.Find(_authorizationService.UserId);
		if (supervisor is null)
			throw new NotFoundException("HTTP User id", "Supervisor");

		var userGroups = queryable
			.Select(e => e.GroupId)
			.Distinct()
			.ToList();

		bool isAuthorized = userGroups.Any(ug => ug != supervisor.GroupId);
		if (isAuthorized)
			throw new ForbidException("Supervisor can edit work time of employees only in their own group");
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

		DbContext.BalanceLogs.Add(balanceLog);
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
