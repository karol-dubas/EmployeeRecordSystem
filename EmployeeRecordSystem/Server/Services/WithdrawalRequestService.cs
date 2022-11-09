using System.Linq.Expressions;
using AutoMapper;
using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Server.Exceptions;
using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.Shared.Requests;
using EmployeeRecordSystem.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Extensions;
using static EmployeeRecordSystem.Server.Installers.Helpers.ServiceAttributes;

namespace EmployeeRecordSystem.Server.Services;

public interface IWithdrawalRequestService
{
	PagedContent<WithdrawalRequestDto> GetAll(WithdrawalRequestQuery query);
	WithdrawalRequestDto Create(Guid employeeId, CreateWithdrawalRequestRequest request);
	void Process(Guid withdrawalRequestId, ProcessWithdrawalRequestRequest request);
}

[ScopedRegistration]
public class WithdrawalRequestService : BaseService, IWithdrawalRequestService
{
	private readonly IAuthorizationService _authorizationService;

	public WithdrawalRequestService(
		ApplicationDbContext dbContext,
		IMapper mapper,
		IAuthorizationService authorizationService) : base(dbContext, mapper)
	{
		_authorizationService = authorizationService;
	}

	public WithdrawalRequestDto Create(Guid employeeId, CreateWithdrawalRequestRequest request)
	{
		var employee = DbContext.Users.Find(employeeId);

		if (employee is null)
			throw new NotFoundException(nameof(employeeId), "Employee");

		var withdrawalRequest = new WithdrawalRequest
		{
			Employee = employee,
			Amount = request.Amount,
			WithdrawalRequestStatusTypeCode = WithdrawalRequestStatusType.Pending.Code
		};

		DbContext.WithdrawalRequests.Add(withdrawalRequest);
		SaveChanges();

		return Mapper.Map<WithdrawalRequestDto>(withdrawalRequest);
	}

	public PagedContent<WithdrawalRequestDto> GetAll(WithdrawalRequestQuery query)
	{
		bool isAuthorized = _authorizationService.IsUserOwnResource(query.EmployeeId) ||
		                    _authorizationService.IsAdmin();
		if (!isAuthorized)
			throw new ForbidException();

		var queryable = DbContext.WithdrawalRequests
			.Include(wr => wr.Employee)
			.AsNoTracking();

		(queryable, int totalItemsCount) = ApplyGetAllQuery(query, queryable);

		var withdrawalRequests = queryable.ToList();
		var dtos = Mapper.Map<List<WithdrawalRequestDto>>(withdrawalRequests);
		return new PagedContent<WithdrawalRequestDto>(dtos, totalItemsCount);
	}

	public void Process(Guid withdrawalRequestId, ProcessWithdrawalRequestRequest request)
	{
		var withdrawalRequest = DbContext.WithdrawalRequests
			.Include(wr => wr.Employee)
			.ThenInclude(u => u.EmployeeBilling)
			.SingleOrDefault(wr => wr.Id == withdrawalRequestId);

		if (withdrawalRequest is null)
			throw new NotFoundException(nameof(withdrawalRequestId), "Withdrawal request");

		if (withdrawalRequest.Employee is null)
			throw new NotFoundException("Withdrawal request employee", "Employee");

		if (withdrawalRequest.IsAlreadyProcessed())
			throw new BadRequestException(nameof(withdrawalRequestId), "Withdrawal request already processed");

		if (WithdrawalRequestAccepted(request))
			AcceptWithdrawalRequest(withdrawalRequest);
		else if (WithdrawalRequestDenied(request))
			DenyWithdrawalRequest(withdrawalRequest);
		else
			return;

		withdrawalRequest.ProcessedAt = DateTimeOffset.Now;
		SaveChanges();
	}

	private static bool WithdrawalRequestDenied(ProcessWithdrawalRequestRequest request)
	{
		return request.ChangeStatusTo == WithdrawalRequestStatusTypeCodes.Denied;
	}

	private static bool WithdrawalRequestAccepted(ProcessWithdrawalRequestRequest request)
	{
		return request.ChangeStatusTo == WithdrawalRequestStatusTypeCodes.Accepted;
	}

	private void DenyWithdrawalRequest(WithdrawalRequest withdrawalRequest)
	{
		withdrawalRequest.WithdrawalRequestStatusTypeCode = WithdrawalRequestStatusTypeCodes.Denied;
	}

	private void AcceptWithdrawalRequest(WithdrawalRequest withdrawalRequest)
	{
		(decimal balanceBefore, decimal balanceAfter) = SubtractUsersBalance(withdrawalRequest);

		var balanceLog = CreateBalanceLog(withdrawalRequest, balanceBefore, balanceAfter);
		DbContext.BalanceLogs.Add(balanceLog);

		withdrawalRequest.WithdrawalRequestStatusTypeCode = WithdrawalRequestStatusTypeCodes.Accepted;
	}

	private BalanceLog CreateBalanceLog(
		WithdrawalRequest withdrawalRequest,
		decimal balanceBefore,
		decimal balanceAfter)
	{
		return new BalanceLog
		{
			BalanceBefore = balanceBefore,
			BalanceAfter = balanceAfter,
			CreatedAt = DateTimeOffset.Now,
			Employee = withdrawalRequest.Employee
		};
	}

	private (decimal, decimal) SubtractUsersBalance(WithdrawalRequest withdrawalRequest)
	{
		decimal balanceBefore = withdrawalRequest.Employee.EmployeeBilling.Balance;
		decimal balanceAfter = withdrawalRequest.Employee.EmployeeBilling.Balance -= withdrawalRequest.Amount;

		if (balanceAfter < 0)
			throw new BadRequestException("User balance",
				"The withdrawal amount must be equal to or greater than the balance");

		return (balanceBefore, balanceAfter);
	}

	private static (IQueryable<WithdrawalRequest> queryable, int totalItemsCount) ApplyGetAllQuery(
		WithdrawalRequestQuery query,
		IQueryable<WithdrawalRequest> queryable)
	{
		if (query.EmployeeId != default)
			queryable = queryable.Where(wr => wr.EmployeeId == query.EmployeeId);

		if (query.WithdrawalRequestStatus != default)
			queryable = queryable.Where(wr => wr.WithdrawalRequestStatusTypeCode == query.WithdrawalRequestStatus);

		if (query.Id != default)
			queryable = queryable.Where(wr => wr.Id == query.Id);

		if (query.NameSearch != default)
			queryable = queryable.Where(wr => wr.Employee.FirstName.ToLower().Contains(query.NameSearch.ToLower()) 
			                               || wr.Employee.LastName.ToLower().Contains(query.NameSearch.ToLower()));

		if (!string.IsNullOrEmpty(query.SortBy))
		{
			var columnsSelector = new Dictionary<string, Expression<Func<WithdrawalRequest, object>>>()
			{
				{ nameof(WithdrawalRequest.CreatedAt), wr => wr.CreatedAt },
				{ nameof(WithdrawalRequest.WithdrawalRequestStatusTypeCode), wr => wr.WithdrawalRequestStatusTypeCode }
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
}
