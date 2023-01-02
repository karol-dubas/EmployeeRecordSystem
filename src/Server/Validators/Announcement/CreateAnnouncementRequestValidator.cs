using EmployeeRecordSystem.Shared.Requests;
using FluentValidation;

namespace EmployeeRecordSystem.Server.Validators.Announcement;

public class CreateAnnouncementRequestValidator : AbstractValidator<CreateAnnouncementRequest>
{
	public CreateAnnouncementRequestValidator()
	{
		RuleFor(r => r.Title)
			.NotEmpty()
			.MaximumLength(50);
		
		RuleFor(r => r.Text)
			.MaximumLength(1000);
	}
}
