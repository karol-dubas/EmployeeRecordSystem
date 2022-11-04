using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.Shared.Requests;
using FluentValidation;

namespace EmployeeRecordSystem.Server.Validators.Group;

public class GroupQueryValidator : AbstractValidator<GroupQuery>
{
	public GroupQueryValidator(ApplicationDbContext dbContext)
	{
		RuleFor(e => e.Id)
			.Custom((value, context) =>
			{
				if (value == default)
					return;
				
				bool exists = dbContext.Groups.Any(g => g.Id == value);
				if (!exists)
					context.AddFailure(nameof(GroupQuery.Id), "Group doesn't exist");
			});
	}
}
