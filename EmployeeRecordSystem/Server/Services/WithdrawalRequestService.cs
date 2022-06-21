using AutoMapper;
using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Queries;
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
    public interface IWithdrawalRequestService
    {
        List<WithdrawalRequestDto> GetAll(WithdrawalRequestQuery query);
        CreatedWithdrawalRequestDto Create(Guid employeeId, CreateWithdrawalRequestRequest request);
        void Process(Guid withdrawalRequestId, ProcessWithdrawalRequestRequest request);
    }

    [ScopedRegistration]
    public class WithdrawalRequestService : BaseService, IWithdrawalRequestService
    {
        public WithdrawalRequestService(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

        public CreatedWithdrawalRequestDto Create(Guid employeeId, CreateWithdrawalRequestRequest request)
        {
            var employee = _dbContext.Users.Find(employeeId);

            // TODO: employee null check

            var withdrawalRequest = new WithdrawalRequest()
            {
                User = employee,
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
                .Include(wr => wr.User)
                .AsNoTracking();

            queryable = ApplyGetAllFilter(query, queryable);

            var withdrawalRequests = queryable.ToList();
            return _mapper.Map<List<WithdrawalRequestDto>>(withdrawalRequests);
        }

        public void Process(Guid withdrawalRequestId, ProcessWithdrawalRequestRequest request)
        {
            var withdrawalRequest = _dbContext.WithdrawalRequests
                .Include(wr => wr.User)
                .ThenInclude(u => u.UserBilling)
                .SingleOrDefault(wr => wr.Id == withdrawalRequestId);

            // TODO: null check withdrawalRequest & withdrawalRequest.User

            if (withdrawalRequest.IsAlreadyProcessed())
            {
                // TODO: throw exception (withdrawal request already processed)
            }

            if (WithdrawalRequestAccepted(request))
            {
                AcceptWithdrawalRequest(withdrawalRequest);
            }
            else if (WithdrawalRequestDenied(request))
            {
                DenyWithdrawalRequest(withdrawalRequest);
            }
            else
            {
                return;
            }

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

        private BalanceLog CreateBalanceLog(WithdrawalRequest withdrawalRequest, decimal balanceBefore, decimal balanceAfter)
        {
            return new BalanceLog()
            {
                BalanceBefore = balanceBefore,
                BalanceAfter = balanceAfter,
                CreatedAt = DateTimeOffset.UtcNow,
                User = withdrawalRequest.User
            };
        }

        private Tuple<decimal, decimal> SubtractUsersBalance(WithdrawalRequest withdrawalRequest)
        {
            decimal balanceBefore = withdrawalRequest.User.UserBilling.Balance;
            decimal balanceAfter = withdrawalRequest.User.UserBilling.Balance -= withdrawalRequest.Amount;

            if (balanceAfter < 0)
            {
                //TODO: throw exception (balance < 0)
            }

            return Tuple.Create(balanceBefore, balanceAfter);
        }

        private IQueryable<WithdrawalRequest> ApplyGetAllFilter(WithdrawalRequestQuery query, IQueryable<WithdrawalRequest> queryable)
        {
            if (query.EmployeeId != default)
            {
                // TODO: employee null check

                queryable = queryable
                    .Include(wr => wr.User)
                    .Where(wr => wr.UserId == query.EmployeeId);
            }

            if (query.WithdrawalRequestStatus != default)
            {
                queryable = queryable
                    .Where(wr => wr.WithdrawalRequestStatusTypeCode == query.WithdrawalRequestStatus);
            }

            if (query.Id != default)
            {
                queryable = queryable
                    .Where(wr => wr.Id == query.Id);
            }

            return queryable;
        }
    }
}
