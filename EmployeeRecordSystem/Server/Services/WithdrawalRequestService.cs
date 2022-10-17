using AutoMapper;
using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Server.Exceptions;
using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.Shared.Requests;
using EmployeeRecordSystem.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using static EmployeeRecordSystem.Server.Installers.ServiceAttributes;

namespace EmployeeRecordSystem.Server.Services;

public interface IWithdrawalRequestService
{
    List<WithdrawalRequestDto> GetAll(WithdrawalRequestQuery query);
    CreatedWithdrawalRequestDto Create(Guid employeeId, CreateWithdrawalRequestRequest request);
    void Process(Guid withdrawalRequestId, ProcessWithdrawalRequestRequest request);
}

[ScopedRegistration]
public class WithdrawalRequestService : BaseService, IWithdrawalRequestService
{
    private readonly IAuthorizationService _authorizationService;

    public WithdrawalRequestService(
        ApplicationDbContext dbContext,
        IMapper mapper,
        IAuthorizationService authorizationService): base(dbContext, mapper)
    {
        _authorizationService = authorizationService;
    }

    public CreatedWithdrawalRequestDto Create(Guid employeeId, CreateWithdrawalRequestRequest request)
    {
        var employee = _dbContext.Users.Find(employeeId);

        if (employee is null)
            throw new NotFoundException("Employee");

        var withdrawalRequest = new WithdrawalRequest
        {
            Employee = employee,
            Amount = request.Amount,
            WithdrawalRequestStatusTypeCode = WithdrawalRequestStatusType.Pending.Code
        };

        _dbContext.WithdrawalRequests.Add(withdrawalRequest);
        SaveChanges();

        return _mapper.Map<CreatedWithdrawalRequestDto>(withdrawalRequest);
    }

    public List<WithdrawalRequestDto> GetAll(WithdrawalRequestQuery query)
    {
        var queryable = _dbContext.WithdrawalRequests
            .Include(wr => wr.Employee)
            .AsNoTracking();

        queryable = ApplyGetAllFilter(query, queryable);

        var withdrawalRequests = queryable.ToList();
        return _mapper.Map<List<WithdrawalRequestDto>>(withdrawalRequests);
    }

    public void Process(Guid withdrawalRequestId, ProcessWithdrawalRequestRequest request)
    {
        var withdrawalRequest = _dbContext.WithdrawalRequests
            .Include(wr => wr.Employee)
            .ThenInclude(u => u.EmployeeBilling)
            .SingleOrDefault(wr => wr.Id == withdrawalRequestId);

        if (withdrawalRequest is null)
            throw new NotFoundException("Withdrawal request");

        if (withdrawalRequest.Employee is null)
            throw new NotFoundException("Employee");

        if (withdrawalRequest.IsAlreadyProcessed())
            throw new InvalidOperationException("Withdrawal request already processed");

        if (WithdrawalRequestAccepted(request))
            AcceptWithdrawalRequest(withdrawalRequest);
        else if (WithdrawalRequestDenied(request))
            DenyWithdrawalRequest(withdrawalRequest);
        else
            return;

        withdrawalRequest.ProcessedAt = DateTimeOffset.UtcNow;
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
        _dbContext.BalanceLogs.Add(balanceLog);

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
            CreatedAt = DateTimeOffset.UtcNow,
            Employee = withdrawalRequest.Employee
        };
    }

    private Tuple<decimal, decimal> SubtractUsersBalance(WithdrawalRequest withdrawalRequest)
    {
        decimal balanceBefore = withdrawalRequest.Employee.EmployeeBilling.Balance;
        decimal balanceAfter = withdrawalRequest.Employee.EmployeeBilling.Balance -= withdrawalRequest.Amount;

        if (balanceAfter < 0)
            throw new ArgumentOutOfRangeException("The withdrawal amount must be equal to or greater than the balance");

        return Tuple.Create(balanceBefore, balanceAfter);
    }

    private IQueryable<WithdrawalRequest> ApplyGetAllFilter(
        WithdrawalRequestQuery query,
        IQueryable<WithdrawalRequest> queryable)
    {
        if (query.EmployeeId != default)
        {
            bool employeeExists = _dbContext.Users.Any(e => e.Id == query.EmployeeId);
            if (!employeeExists)
                throw new NotFoundException("Employee");

            queryable = queryable
                .Include(wr => wr.Employee)
                .Where(wr => wr.EmployeeId == query.EmployeeId);
        }

        if (query.WithdrawalRequestStatus != default)
            queryable = queryable.Where(wr => wr.WithdrawalRequestStatusTypeCode == query.WithdrawalRequestStatus);

        if (query.Id != default)
            queryable = queryable.Where(wr => wr.Id == query.Id);

        return queryable;
    }
}