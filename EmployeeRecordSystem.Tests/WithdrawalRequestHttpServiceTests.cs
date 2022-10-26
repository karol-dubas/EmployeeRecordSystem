using System.Net;
using EmployeeRecordSystem.Client.HttpServices;
using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Tests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using EmployeeRecordSystem.Server;
using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.Shared.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeRecordSystem.Tests;

public class WithdrawalRequestHttpServiceTests : IntegrationTest
{
	private readonly WithdrawalRequestHttpService _sut;

	public WithdrawalRequestHttpServiceTests()
	{
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder =>
			{
				builder.ConfigureServices(services =>
				{
					var dbContextOptions = services.Single(service =>
						service.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
					services.Remove(dbContextOptions);
					services.AddDbContext<ApplicationDbContext>(o =>
					{
						string uniqueCurrentClassName = GetType().Name;
						o.UseInMemoryDatabase(uniqueCurrentClassName);
					});

					services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
				});
			});

		var httpClient = factory.CreateClient();
		_sut = new WithdrawalRequestHttpService(httpClient);

		DbContext = GetDbContext(factory);
	}
	
	[Fact]
	public async Task CreateAsync_ForValidInput_ReturnsOkWithWithdrawalRequests()
	{
		// Arrange
		var employee = SeedEmployee();
		const int amount = 10;
		SeedEmployeeBilling(employee);
		var request = new CreateWithdrawalRequestRequest { Amount = amount };
		    
		// Act
		var response = await _sut.CreateAsync(employee.Id, request);
	
		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.Created);
		response.DeserializedContent.Amount.Should().Be(amount);
	}
	
	[Fact]
	public async Task CreateAsync_ForInvalidInput_ReturnsNotFound()
	{
		// Arrange
		var invalidEmployeeId = Guid.NewGuid();
		    
		// Act
		var response = await _sut.CreateAsync(invalidEmployeeId, new CreateWithdrawalRequestRequest());
	
		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
	
	[Fact]
	public async Task GetAllAsync_ForValidInput_ReturnsOkWithWithdrawalRequests()
	{
	    // Arrange
	    var employee = SeedEmployee();
	    SeedWithdrawalRequest(employee.Id);
	    var query = new WithdrawalRequestQuery { EmployeeId = employee.Id  };
		    
	    // Act
	    var response = await _sut.GetAllAsync(query);

	    // Assert
	    response.StatusCode.Should().Be(HttpStatusCode.OK);
	    response.DeserializedContent.Should().HaveCount(1);
	}
	
	[Fact]
	public async Task ProcessAsync_ForValidInput_ReturnsNoContent()
	{
	    // Arrange
	    var employee = SeedEmployee();
	    var withdrawalRequest = SeedWithdrawalRequest(employee.Id);
	    var request = new ProcessWithdrawalRequestRequest { ChangeStatusTo = WithdrawalRequestStatusTypeCodes.Accepted };

	    // Act
	    var response = await _sut.ProcessAsync(withdrawalRequest.Id, request);

	    // Assert
	    response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task ProcessAsync_ForInvalidInput_ReturnsNotFound()
	{
		// Arrange
		var invalidWithdrawalRequestId = Guid.NewGuid();
		var request = new ProcessWithdrawalRequestRequest { ChangeStatusTo = WithdrawalRequestStatusTypeCodes.Accepted };
	    
		// Act
		var response = await _sut.ProcessAsync(invalidWithdrawalRequestId, request);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
}
