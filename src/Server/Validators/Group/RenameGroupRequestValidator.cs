using EmployeeRecordSystem.Shared.Requests;
using FluentValidation;

namespace EmployeeRecordSystem.Server.Validators.Group;

public class RenameGroupRequestValidator : AbstractValidator<RenameGroupRequest>
{
	public RenameGroupRequestValidator()
	{
		RuleFor(e => e.Name)
			.NotEmpty()
			.MaximumLength(50);
	}
}
