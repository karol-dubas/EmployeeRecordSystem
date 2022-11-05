using EmployeeRecordSystem.Server.Validators.Employee;
using EmployeeRecordSystem.Server.Validators.Group;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.UnitTests.Helpers;
using FluentValidation.TestHelper;

namespace EmployeeRecordSystem.UnitTests.Validators.Group;

public class GroupQueryValidatorTests : MockDatabase
{
	private readonly GroupQueryValidator _sut;
	
	public GroupQueryValidatorTests()
	{
		_sut = new GroupQueryValidator(DbContextMock.Object);
	}
	
	[Theory]
	[MemberData(nameof(GetValidRequests))]
	public void Validate_ForValidModel_ReturnsSuccess(GroupQuery query)
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
		var query = new GroupQuery()
		{
			Id = invalidGroupId
		};
		
		// Act
		var result = _sut.TestValidate(query);

		// Assert
		result.ShouldHaveValidationErrorFor(nameof(GroupQuery.Id));
	}
	
	public static IEnumerable<object[]> GetValidRequests()
	{
		var list = new List<GroupQuery>
		{
			new()
			{
				Id = Guid.Empty
			},
			new()
			{
				Id = GroupMock.Id
			}
		};

		return list.Select(x => new object[] { x });
	}
}
