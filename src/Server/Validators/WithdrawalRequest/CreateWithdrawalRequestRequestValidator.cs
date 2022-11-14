using EmployeeRecordSystem.Shared.Requests;
using FluentValidation;

namespace EmployeeRecordSystem.Server.Validators.WithdrawalRequest;

public class CreateWithdrawalRequestRequestValidator : AbstractValidator<CreateWithdrawalRequestRequest>
{
	public CreateWithdrawalRequestRequestValidator()
	{
		RuleFor(e => e.Amount).GreaterThan(0);
	}
}
