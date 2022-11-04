using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Shared.Queries;
using FluentValidation;

namespace EmployeeRecordSystem.Server.Validators.Employee;

public class EmployeeQueryValidator : AbstractValidator<EmployeeQuery>
{
	public EmployeeQueryValidator(ApplicationDbContext dbContext)
	{
		RuleFor(e => e.GroupId)                
			.Custom((value, context) =>
			{
				if (value == default)
					return;
				
				bool exists = dbContext.Groups.Any(g => g.Id == value);
				if (!exists)
					context.AddFailure(nameof(EmployeeQuery.GroupId), "Group doesn't exist");
			});
	}
}
