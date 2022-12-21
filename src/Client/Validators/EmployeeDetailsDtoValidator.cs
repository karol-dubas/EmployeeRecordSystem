using EmployeeRecordSystem.Shared.Responses;
using FluentValidation;

namespace EmployeeRecordSystem.Client.Validators;

public class EmployeeDetailsDtoValidator : AbstractValidator<EmployeeDetailsDto>
{
	public EmployeeDetailsDtoValidator()
	{
		RuleFor(e => e.FirstName)
			.MaximumLength(50).WithMessage("Maksymalna długość to 50 znaków")
			.NotEmpty().WithMessage("Pole nie może być puste");

		RuleFor(e => e.LastName)
			.MaximumLength(50).WithMessage("Maksymalna długość to 50 znaków")
			.NotEmpty().WithMessage("Pole nie może być puste");

		RuleFor(e => e.BankAccountNumber)
			.MaximumLength(34).WithMessage("Maksymalna długość to 34 znaki");

		RuleFor(e => e.Note)
			.MaximumLength(300).WithMessage("Maksymalna długość to 300 znaków");
	}

	public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
	{
		var result = await ValidateAsync(
			ValidationContext<EmployeeDetailsDto>.CreateWithOptions((EmployeeDetailsDto)model, x => x.IncludeProperties(propertyName)));
		return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
	};
}
