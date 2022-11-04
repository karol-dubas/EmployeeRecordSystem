using EmployeeRecordSystem.Server.Validators.Employee;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.UnitTests.Helpers;
using FluentValidation.TestHelper;

namespace EmployeeRecordSystem.UnitTests.Validators.Employee;

public class EmployeeQueryValidatorTests : MockDatabase
{
	private readonly EmployeeQueryValidator _sut;
	
	public EmployeeQueryValidatorTests()
	{
		_sut = new EmployeeQueryValidator(DbContextMock.Object);
	}
	
	[Theory]
	[MemberData(nameof(GetValidRequests))]
	public void Validate_ForValidModel_ReturnsSuccess(EmployeeQuery query)
	{
	    // Arrange
	    
	    // Act
	    var result = _sut.TestValidate(query);

	    // Assert
	    result.ShouldNotHaveAnyValidationErrors();
	}
	
	[Fact]
	public void Validate_ForInvalidModel_ReturnsError()
	{
		// Arrange
		var invalidGroupId = Guid.NewGuid();
		var query = new EmployeeQuery()
		{
			GroupId = invalidGroupId
		};
		
		// Act
		var result = _sut.TestValidate(query);

		// Assert
		result.ShouldHaveValidationErrorFor(nameof(EmployeeQuery.GroupId));
	}
	
	public static IEnumerable<object[]> GetValidRequests()
	{
		var list = new List<EmployeeQuery>
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
