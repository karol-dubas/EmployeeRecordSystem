using EmployeeRecordSystem.Server.Validators.WithdrawalRequest;
using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.UnitTests.Helpers;
using FluentValidation.TestHelper;

namespace EmployeeRecordSystem.UnitTests.Validators.WithdrawalRequest;

public class WithdrawalRequestQueryValidatorTests : MockDatabase
{
	private readonly WithdrawalRequestQueryValidator _sut;
	
	public WithdrawalRequestQueryValidatorTests()
	{
		_sut = new WithdrawalRequestQueryValidator(DbContextMock.Object);
	}
	
	[Theory]
	[MemberData(nameof(GetValidRequests))]
	public void Validate_ForValidModel_ReturnsSuccess(WithdrawalRequestQuery query)
	{
		// Arrange

		// Act
		var result = _sut.TestValidate(query);

		// Assert
		result.ShouldNotHaveAnyValidationErrors();
	}
	
	[Theory]
	[MemberData(nameof(GetInvalidRequests))]
	public void Validate_ForInvalidModel_ReturnsError(WithdrawalRequestQuery query, string invalidProperty)
	{
		// Arrange

		// Act
		var result = _sut.TestValidate(query);

		// Assert
		result.ShouldHaveValidationErrorFor(invalidProperty);
	}
	
	public static IEnumerable<object[]> GetValidRequests()
	{
		var list = new List<WithdrawalRequestQuery>
		{
			new()
			{
				Id = WithdrawalRequestMock.Id,
				EmployeeId = EmployeeMock.Id,
				WithdrawalRequestStatus = WithdrawalRequestStatusTypeCodes.Accepted
			},
			new()
			{
				Id = Guid.Empty,
				EmployeeId = Guid.Empty,
				WithdrawalRequestStatus = WithdrawalRequestStatusTypeCodes.Denied
			},
			new()
			{
				Id = Guid.Empty,
				EmployeeId = Guid.Empty,
				WithdrawalRequestStatus = WithdrawalRequestStatusTypeCodes.Pending
			},
			new()
			{
				Id = Guid.Empty,
				EmployeeId = Guid.Empty,
				WithdrawalRequestStatus = null
			},
		};

		return list.Select(x => new object[] { x });
	}
	
	public static IEnumerable<object[]> GetInvalidRequests()
	{
		List<(string invalidProperty, WithdrawalRequestQuery request)> requestsWithInvalidProperty = new()
		{
			(
				nameof(WithdrawalRequestQuery.WithdrawalRequestStatus),
				new WithdrawalRequestQuery()
				{
					WithdrawalRequestStatus = ""
				}
			),
			(
				nameof(WithdrawalRequestQuery.Id),
				new WithdrawalRequestQuery()
				{
					Id = Guid.NewGuid(),
				}
			),
			(
				nameof(WithdrawalRequestQuery.EmployeeId),
				new WithdrawalRequestQuery()
				{
					EmployeeId = Guid.NewGuid(),
				}
			),
		};

		return requestsWithInvalidProperty.Select(x => new object[] { x.request, x.invalidProperty });
	}
}
