using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Requests;
using FluentValidation;

namespace EmployeeRecordSystem.Server.Validators.WithdrawalRequest;

public class ProcessWithdrawalRequestRequestValidator : AbstractValidator<ProcessWithdrawalRequestRequest>
{
	public ProcessWithdrawalRequestRequestValidator()
	{
		string[] availableWithdrawalRequestStatuses =
		{
			WithdrawalRequestStatusTypeCodes.Accepted,
			WithdrawalRequestStatusTypeCodes.Denied,
		};

		RuleFor(e => e.ChangeStatusTo)
			.Must(p => availableWithdrawalRequestStatuses.Contains(p))
			.WithMessage($"{nameof(ProcessWithdrawalRequestRequest.ChangeStatusTo)} " +
			             "must be one of these values: " +
			             $"{string.Join(", ", availableWithdrawalRequestStatuses)}");
	}
}
