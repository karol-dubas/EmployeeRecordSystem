using FluentValidation;

namespace EmployeeRecordSystem.Client.Validators;

/// <summary>
/// A glue class to make it easy to define validation rules for single values using FluentValidation.
/// </summary>
/// <typeparam name="T">Type of variable to be validated</typeparam>
public class FluentValueValidator<T> : AbstractValidator<T>
{
	public FluentValueValidator(Action<IRuleBuilderInitial<T, T>> rule)
	{
		rule(RuleFor(x => x));
	}

	private IEnumerable<string> ValidateValue(T arg)
	{
		var result = Validate(arg);
		return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
	}

	public Func<T, IEnumerable<string>> Validation => ValidateValue;
}
