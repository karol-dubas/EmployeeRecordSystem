using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Requests;
using FluentValidation;

namespace EmployeeRecordSystem.Server.Validators.Employee;

public class ChangeEmployeesWorkTimeRequestValidator : AbstractValidator<ChangeEmployeesWorkTimeRequest>
{
	public ChangeEmployeesWorkTimeRequestValidator(ApplicationDbContext dbContext)
	{
		string[] availableWorkTimeOperations = { WorkTimeOperations.Add, WorkTimeOperations.Subtract };

		RuleFor(e => e.WorkTimeOperation)
			.Must(p => availableWorkTimeOperations.Contains(p))
			.WithMessage($"{nameof(ChangeEmployeesWorkTimeRequest.WorkTimeOperation)} must be one of these values: " +
			             $"{string.Join(", ", availableWorkTimeOperations)}");

		RuleFor(e => e.EmployeeIds)
			.Must(p => p.Count > 0)
			.WithMessage($"{nameof(ChangeEmployeesWorkTimeRequest.EmployeeIds)}" +
			             $" list should contain at least one element");
		
		RuleFor(e => e.EmployeeIds)
			.Custom((value, context) =>
			{
				bool allExist = dbContext.Users.Count(u => value.Contains(u.Id)) == value.Count;
				if (!allExist)
					context.AddFailure(nameof(ChangeEmployeesWorkTimeRequest.EmployeeIds),
						"Employee doesn't exist");
			});

		RuleFor(e => e.WorkTime).GreaterThan(TimeSpan.Zero);
	}
}
