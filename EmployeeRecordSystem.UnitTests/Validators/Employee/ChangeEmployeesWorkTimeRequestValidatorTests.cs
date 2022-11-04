using EmployeeRecordSystem.Server.Validators.Employee;
using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Requests;
using EmployeeRecordSystem.UnitTests.Helpers;
using FluentValidation.TestHelper;

namespace EmployeeRecordSystem.UnitTests.Validators.Employee;

public class ChangeEmployeesWorkTimeRequestValidatorTests : MockDatabase
{
	private readonly ChangeEmployeesWorkTimeRequestValidator _sut;

	public ChangeEmployeesWorkTimeRequestValidatorTests()
	{
		_sut = new ChangeEmployeesWorkTimeRequestValidator(DbContextMock.Object);
	}

	[Theory]
	[InlineData(WorkTimeOperations.Add)]
	[InlineData(WorkTimeOperations.Subtract)]
	public void Validate_ForValidModel_ReturnsSuccess(string workTimeOperation)
	{
		// Arrange
		var request = new ChangeEmployeesWorkTimeRequest()
		{
			EmployeeIds = new List<Guid>() { EmployeeMock.Id },
			WorkTimeOperation = workTimeOperation,
			WorkTime = TimeSpan.FromSeconds(1)
		};

		// Act
		var result = _sut.TestValidate(request);

		// Assert
		result.ShouldNotHaveAnyValidationErrors();
	}

	[Theory]
	[MemberData(nameof(GetInvalidRequests))]
	public void Validate_ForInvalidModel_ReturnsError(ChangeEmployeesWorkTimeRequest request, string invalidProperty)
	{
		// Arrange

		// Act
		var result = _sut.TestValidate(request);

		// Assert
		result.ShouldHaveValidationErrorFor(invalidProperty);
	}

	public static IEnumerable<object[]> GetInvalidRequests()
	{
		List<(string invalidProperty, ChangeEmployeesWorkTimeRequest request)> requestsWithInvalidProperty = new()
		{
			(
				nameof(ChangeEmployeesWorkTimeRequest.EmployeeIds),
				new ChangeEmployeesWorkTimeRequest()
				{
					EmployeeIds = new List<Guid>(),
				}
			),
			(
				nameof(ChangeEmployeesWorkTimeRequest.EmployeeIds),
				new ChangeEmployeesWorkTimeRequest()
				{
					EmployeeIds = new List<Guid> { Guid.NewGuid() },
				}
			),
			(
				nameof(ChangeEmployeesWorkTimeRequest.WorkTimeOperation),
				new ChangeEmployeesWorkTimeRequest()
				{
					WorkTimeOperation = null,
				}
			),
			(
				nameof(ChangeEmployeesWorkTimeRequest.WorkTimeOperation),
				new ChangeEmployeesWorkTimeRequest()
				{
					WorkTimeOperation = "",
				}
			),
			(
				nameof(ChangeEmployeesWorkTimeRequest.WorkTime),
				new ChangeEmployeesWorkTimeRequest()
				{
					WorkTime = TimeSpan.FromSeconds(-1)
				}
			)
		};

		return requestsWithInvalidProperty.Select(x => new object[] { x.request, x.invalidProperty });
	}
}
