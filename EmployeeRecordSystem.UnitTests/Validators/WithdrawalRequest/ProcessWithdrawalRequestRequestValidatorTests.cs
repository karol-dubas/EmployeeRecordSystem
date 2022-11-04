using EmployeeRecordSystem.Server.Validators.WithdrawalRequest;
using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Requests;
using EmployeeRecordSystem.UnitTests.Helpers;
using FluentValidation.TestHelper;

namespace EmployeeRecordSystem.UnitTests.Validators.WithdrawalRequest;

public class ProcessWithdrawalRequestRequestValidatorTests
{
	private readonly ProcessWithdrawalRequestRequestValidator _sut;
	
	public ProcessWithdrawalRequestRequestValidatorTests()
	{
		_sut = new ProcessWithdrawalRequestRequestValidator();
	}
	
	[Theory]
	[InlineData(WithdrawalRequestStatusTypeCodes.Accepted)]
	[InlineData(WithdrawalRequestStatusTypeCodes.Denied)]
	public void Validate_ForValidModel_ReturnsSuccess(string status)
	{
	    // Arrange
	    var request = new ProcessWithdrawalRequestRequest
	    {
		    ChangeStatusTo = status
	    };

	    // Act
	    var result = _sut.TestValidate(request);

	    // Assert
		result.ShouldNotHaveAnyValidationErrors();
	}
	
	[Theory]
	[InlineData(WithdrawalRequestStatusTypeCodes.Pending)]
	[InlineData("")]
	[InlineData(null)]
	public void Validate_ForInvalidModel_ReturnsError(string status)
	{
		// Arrange
		var request = new ProcessWithdrawalRequestRequest
		{
			ChangeStatusTo = status
		};

		// Act
		var result = _sut.TestValidate(request);

		// Assert
		result.ShouldHaveValidationErrorFor(nameof(ProcessWithdrawalRequestRequest.ChangeStatusTo));
	}
}
