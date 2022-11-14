using EmployeeRecordSystem.Server.Validators.Employee;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.UnitTests.Helpers;
using FluentValidation.TestHelper;
using MudBlazor;
using MudBlazor.Extensions;

namespace EmployeeRecordSystem.UnitTests.Validators.Employee;

public class BalanceLogQueryValidatorTests
{
	private readonly BalanceLogQueryValidator _sut;

	public BalanceLogQueryValidatorTests()
	{
		_sut = new BalanceLogQueryValidator();
	}
	
	[Theory]
	[MemberData(nameof(GetValidRequests))]
	public void Validate_ForValidModel_ReturnsSuccess(BalanceLogQuery query)
	{
		// Arrange

		// Act
		var result = _sut.TestValidate(query);

		// Assert
		result.ShouldNotHaveAnyValidationErrors();
	}
	
	[Theory]
	[MemberData(nameof(GetInvalidRequests))]
	public void Validate_ForInvalidModel_ReturnsError(BalanceLogQuery query, string invalidProperty)
	{
		// Arrange

		// Act
		var result = _sut.TestValidate(query);

		// Assert
		result.ShouldHaveValidationErrorFor(invalidProperty);
	}
	
	public static IEnumerable<object[]> GetValidRequests()
	{
		var list = new List<BalanceLogQuery>
		{
			new()
			{
				SortDirection = SortDirection.Ascending.ToDescriptionString(),
				SortBy = nameof(Data.Entities.BalanceLog.CreatedAt),
				PageSize = 1,
				PageNumber = 1,
			},
			new()
			{
				SortDirection = SortDirection.Descending.ToDescriptionString(),
				SortBy = nameof(Data.Entities.BalanceLog.BalanceAfter),
				PageSize = 10,
				PageNumber = 10,
			},
			new()
			{
				SortDirection = SortDirection.None.ToDescriptionString(),
				SortBy = nameof(Data.Entities.BalanceLog.BalanceAfter),
				PageSize = 100,
				PageNumber = 100,
			},
		};

		return list.Select(x => new object[] { x });
	}
	
	public static IEnumerable<object[]> GetInvalidRequests()
	{
		List<(string invalidProperty, BalanceLogQuery request)> requestsWithInvalidProperty = new()
		{
			(
				nameof(BalanceLogQuery.SortBy),
				new BalanceLogQuery()
				{
					SortBy = "invalid"
				}
			),
			(
				nameof(BalanceLogQuery.SortDirection),
				new BalanceLogQuery()
				{
					SortDirection = "invalid"
				}
			),
			(
				nameof(BalanceLogQuery.PageSize),
				new BalanceLogQuery()
				{
					PageSize = 0
				}
			),
			(
				nameof(BalanceLogQuery.PageNumber),
				new BalanceLogQuery()
				{
					PageNumber = 0
				}
			),
		};

		return requestsWithInvalidProperty.Select(x => new object[] { x.request, x.invalidProperty });
	}
}
