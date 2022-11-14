using EmployeeRecordSystem.Shared.Requests;
using FluentValidation;

namespace EmployeeRecordSystem.Server.Validators.Employee;

public class EditEmployeeRequestValidator : AbstractValidator<EditEmployeeRequest>
{
	public EditEmployeeRequestValidator()
	{
		RuleFor(e => e.FirstName)
			.MaximumLength(50)
			.NotEmpty();
		
		RuleFor(e => e.LastName)			
			.MaximumLength(50)
			.NotEmpty();
		
		RuleFor(e => e.BankAccountNumber).MaximumLength(34);
		
		RuleFor(e => e.Note).MaximumLength(300);
	}
}
