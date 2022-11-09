using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Queries;
using FluentValidation;
using MudBlazor;
using MudBlazor.Extensions;

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

		string[] availableSortByProperties =
		{
			nameof(Data.Entities.WithdrawalRequest.CreatedAt),
			nameof(Data.Entities.WithdrawalRequest.WithdrawalRequestStatusTypeCode)
		};

		string[] availableSortDirections =
		{
			SortDirection.Ascending.ToDescriptionString(),
			SortDirection.Descending.ToDescriptionString(),
			SortDirection.None.ToDescriptionString()
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

		RuleFor(e => e.SortBy).Must(p => availableSortByProperties.Contains(p) || p == default);
		
		RuleFor(e => e.SortDirection).Must(p => availableSortDirections.Contains(p) || p == default);
		
		RuleFor(e => e.PageSize).GreaterThanOrEqualTo(1);

		RuleFor(e => e.PageNumber).GreaterThanOrEqualTo(1);
	}
}
