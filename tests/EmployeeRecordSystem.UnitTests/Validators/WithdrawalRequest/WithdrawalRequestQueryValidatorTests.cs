using EmployeeRecordSystem.Server.Validators.WithdrawalRequest;
using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.UnitTests.Helpers;
using FluentValidation.TestHelper;
using MudBlazor;
using MudBlazor.Extensions;

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
				WithdrawalRequestStatus = WithdrawalRequestStatusTypeCodes.Accepted,
				SortBy = nameof(Data.Entities.WithdrawalRequest.CreatedAt),
				SortDirection = SortDirection.Ascending.ToDescriptionString(),
				PageSize = 1,
				PageNumber = 1
			},
			new()
			{
				Id = Guid.Empty,
				EmployeeId = Guid.Empty,
				WithdrawalRequestStatus = WithdrawalRequestStatusTypeCodes.Denied,
				SortBy = nameof(Data.Entities.WithdrawalRequest.WithdrawalRequestStatusTypeCode),
				SortDirection = SortDirection.Descending.ToDescriptionString(),
				PageSize = 20,
				PageNumber = 20
			},
			new()
			{
				Id = Guid.Empty,
				EmployeeId = Guid.Empty,
				WithdrawalRequestStatus = WithdrawalRequestStatusTypeCodes.Pending,
				SortBy = null,
				SortDirection = SortDirection.None.ToDescriptionString(),
				PageSize = 45,
				PageNumber = 45,
			},
			new()
			{
				Id = Guid.Empty,
				EmployeeId = Guid.Empty,
				WithdrawalRequestStatus = null,
				SortBy = null, 
				SortDirection = null,
				PageSize = 1000,
				PageNumber = 1000,
			},
		};

		return list.Select(x => new object[] { x });
	}
	
	public static IEnumerable<object[]> GetInvalidRequests()
	{
		List<(string invalidProperty, WithdrawalRequestQuery request)> requestsWithInvalidProperty = new()
		{
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
			(
				nameof(WithdrawalRequestQuery.WithdrawalRequestStatus),
				new WithdrawalRequestQuery()
				{
					WithdrawalRequestStatus = "invalid"
				}
			),
			(
				nameof(WithdrawalRequestQuery.SortBy),
				new WithdrawalRequestQuery()
				{
					SortBy = "invalid"
				}
			),
			(
				nameof(WithdrawalRequestQuery.SortDirection),
				new WithdrawalRequestQuery()
				{
					SortDirection = "invalid"
				}
			),
			(
				nameof(WithdrawalRequestQuery.PageSize),
				new WithdrawalRequestQuery()
				{
					PageSize = 0
				}
			),
			(
				nameof(WithdrawalRequestQuery.PageSize),
				new WithdrawalRequestQuery()
				{
					PageNumber = 0
				}
			),
		};

		return requestsWithInvalidProperty.Select(x => new object[] { x.request, x.invalidProperty });
	}
}
