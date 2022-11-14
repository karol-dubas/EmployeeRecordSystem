using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Queries;
using FluentValidation;
using MudBlazor;
using MudBlazor.Extensions;

namespace EmployeeRecordSystem.Server.Validators.Employee;

public class BalanceLogQueryValidator : AbstractValidator<BalanceLogQuery>
{
	public BalanceLogQueryValidator()
	{
		string[] availableSortByProperties =
		{
			nameof(Data.Entities.BalanceLog.CreatedAt),
			nameof(Data.Entities.BalanceLog.BalanceAfter)
		};

		string[] availableSortDirections =
		{
			SortDirection.Ascending.ToDescriptionString(),
			SortDirection.Descending.ToDescriptionString(),
			SortDirection.None.ToDescriptionString()
		};

		RuleFor(e => e.SortBy).Must(p => availableSortByProperties.Contains(p) || p == default);

		RuleFor(e => e.SortDirection).Must(p => availableSortDirections.Contains(p) || p == default);

		RuleFor(e => e.PageSize).GreaterThanOrEqualTo(1);

		RuleFor(e => e.PageNumber).GreaterThanOrEqualTo(1);
	}
}
