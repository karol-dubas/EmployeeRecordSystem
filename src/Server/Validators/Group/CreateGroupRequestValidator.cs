using EmployeeRecordSystem.Shared.Requests;
using FluentValidation;

namespace EmployeeRecordSystem.Server.Validators.Group;

public class CreateGroupRequestValidator : AbstractValidator<CreateGroupRequest>
{
	public CreateGroupRequestValidator()
	{
		RuleFor(e => e.Name)
			.NotEmpty()
			.MaximumLength(50);
	}
}
