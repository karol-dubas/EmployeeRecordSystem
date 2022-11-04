using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.Shared.Requests;
using FluentValidation;

namespace EmployeeRecordSystem.Server.Validators.Employee;

public class ConvertTimeRequestValidator : AbstractValidator<ConvertTimeRequest>
{
	public ConvertTimeRequestValidator(ApplicationDbContext dbContext)
	{
		RuleFor(e => e.GroupId)
			.Custom((value, context) =>
			{
				if (value == default)
					return;
				
				bool exists = dbContext.Groups.Any(g => g.Id == value);
				if (!exists)
					context.AddFailure(nameof(ConvertTimeRequest.GroupId), "Group doesn't exist");
			});
	}
}
