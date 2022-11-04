using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Queries;
using FluentValidation;

namespace EmployeeRecordSystem.Server.Validators.WithdrawalRequest;

public class WithdrawalRequestQueryValidator : AbstractValidator<WithdrawalRequestQuery>
{
	public WithdrawalRequestQueryValidator(ApplicationDbContext dbContext)
	{
		string[] availableWithdrawalRequestStatuses =
		{
			WithdrawalRequestStatusTypeCodes.Accepted,
			WithdrawalRequestStatusTypeCodes.Denied,
			WithdrawalRequestStatusTypeCodes.Pending
		};

		RuleFor(e => e.Id)			
			.Custom((value, context) =>
			{
				if (value == default)
					return;

				bool exist = dbContext.WithdrawalRequests.Any(wr => wr.Id == value);
				if (!exist)
					context.AddFailure(nameof(WithdrawalRequestQuery.Id), 
						"Withdrawal request doesn't exist");
			});

		RuleFor(e => e.EmployeeId)
			.Custom((value, context) =>
			{
				if (value == default)
					return;

				bool exist = dbContext.Users.Any(e => e.Id == value);
				if (!exist)
					context.AddFailure(nameof(WithdrawalRequestQuery.EmployeeId), 
						"Employee doesn't exist");
			});

		RuleFor(e => e.WithdrawalRequestStatus)
			.Must(p => availableWithdrawalRequestStatuses.Contains(p) || p == default)
			.WithMessage($"{nameof(WithdrawalRequestQuery.WithdrawalRequestStatus)} must be one of these values: " +
			             $"{string.Join(", ", availableWithdrawalRequestStatuses)}");
	}
}
