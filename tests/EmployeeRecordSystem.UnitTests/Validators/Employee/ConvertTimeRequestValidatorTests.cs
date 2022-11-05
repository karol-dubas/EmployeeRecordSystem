using EmployeeRecordSystem.Server.Validators.Employee;
using EmployeeRecordSystem.Shared.Requests;
using EmployeeRecordSystem.UnitTests.Helpers;
using FluentValidation.TestHelper;

namespace EmployeeRecordSystem.UnitTests.Validators.Employee;

public class ConvertTimeRequestValidatorTests : MockDatabase
{
	private readonly ConvertTimeRequestValidator _sut;

	public ConvertTimeRequestValidatorTests()
	{
		_sut = new ConvertTimeRequestValidator(DbContextMock.Object);
	}

	[Theory]
	[MemberData(nameof(GetValidRequests))]
	public void Validate_ForValidModel_ReturnsSuccess(ConvertTimeRequest request)
	{
		// Arrange

		// Act
		var result = _sut.TestValidate(request);

		// Assert
		result.ShouldNotHaveAnyValidationErrors();
	}

	[Fact]
	public void Validate_ForInvalidModel_ReturnsError()
	{
		// Arrange
		var invalidGroupId = Guid.NewGuid();
		var request = new ConvertTimeRequest { GroupId = invalidGroupId };

		// Act
		var result = _sut.TestValidate(request);

		// Assert
		result.ShouldHaveValidationErrorFor(nameof(request.GroupId));
	}
	
	public static IEnumerable<object[]> GetValidRequests()
	{
		var list = new List<ConvertTimeRequest>
		{
			new()
			{
				GroupId = Guid.Empty
			},
			new()
			{
				GroupId = GroupMock.Id
			}
		};

		return list.Select(x => new object[] { x });
	}
}