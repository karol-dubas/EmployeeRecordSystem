using EmployeeRecordSystem.Shared.Requests;
using FluentValidation;

namespace EmployeeRecordSystem.Server.Validators.Employee;

public class ChangeEmployeeHourlyPayRequestValidator : AbstractValidator<ChangeEmployeeHourlyPayRequest>
{
	public ChangeEmployeeHourlyPayRequestValidator()
	{
		RuleFor(e => e.HourlyPay).GreaterThan(0);
	}
}
