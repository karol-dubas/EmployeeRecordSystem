﻿using System.Net;
using EmployeeRecordSystem.Client.HttpServices;
using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using EmployeeRecordSystem.Server;
using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.Shared.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MudBlazor;
using MudBlazor.Extensions;
using Program = EmployeeRecordSystem.Server.Program;

namespace EmployeeRecordSystem.IntegrationTests;

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
					
					services.AddMvc(b => b.Filters.Add(new FakeAdminClaimsFilter()));
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
	public async Task CreateAsync_ForInvalidId_ReturnsNotFound()
	{
		// Arrange
		var invalidEmployeeId = Guid.NewGuid();
		var request = new CreateWithdrawalRequestRequest() { Amount = 1 };
		    
		// Act
		var response = await _sut.CreateAsync(invalidEmployeeId, request);
	
		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		response.Errors.Should().HaveCount(1);
	}

	[Fact]
	public async Task CreateAsync_ForInvalidRequest_ReturnsBadRequest()
	{
		// Arrange
		var employee = SeedEmployee();
		var invalidRequest = new CreateWithdrawalRequestRequest() { Amount = -1 };

		// Act
		var response = await _sut.CreateAsync(employee.Id, invalidRequest);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		response.Errors.Should().HaveCount(1);
	}
	
	[Fact]
	public async Task GetAllAsync_ForValidQuery_ReturnsOkWithWithdrawalRequests()
	{
	    // Arrange
	    var employee = SeedEmployee();
	    SeedWithdrawalRequest(employee.Id);
	    var query = new WithdrawalRequestQuery
	    {
		    EmployeeId = employee.Id,
		    SortBy = nameof(WithdrawalRequest.CreatedAt),
		    SortDirection = SortDirection.Ascending.ToDescriptionString(),
		    PageSize = 10,
		    PageNumber = 1
	    };
		    
	    // Act
	    var response = await _sut.GetAllAsync(query);

	    // Assert
	    response.StatusCode.Should().Be(HttpStatusCode.OK);
	    response.DeserializedContent.Items.Should().HaveCount(1);
	}
	
	[Fact]
	public async Task GetAllAsync_ForInvalidQuery_ReturnsBadRequest()
	{
		// Arrange
		var invalidId = Guid.NewGuid();
		var query = new WithdrawalRequestQuery
		{
			Id = invalidId,
			EmployeeId = invalidId,
			WithdrawalRequestStatus = "invalid",
			SortBy = "invalid",
			SortDirection = "invalid",
			PageSize = -1,
			PageNumber = -1,
		};
		    
		// Act
		var response = await _sut.GetAllAsync(query);
	
		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		response.Errors.Should().HaveCount(7);
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
	public async Task ProcessAsync_ForInvalidId_ReturnsNotFound()
	{
		// Arrange
		var invalidWithdrawalRequestId = Guid.NewGuid();
		var request = new ProcessWithdrawalRequestRequest { ChangeStatusTo = WithdrawalRequestStatusTypeCodes.Accepted };
	    
		// Act
		var response = await _sut.ProcessAsync(invalidWithdrawalRequestId, request);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		response.Errors.Should().HaveCount(1);
	}

	[Fact]
	public async Task ProcessAsync_ForInvalidRequest_ReturnsBadRequest()
	{
		// Arrange
		var invalidWithdrawalRequestId = Guid.NewGuid();
		var invalidRequest = new ProcessWithdrawalRequestRequest { ChangeStatusTo = "invalid" };

		// Act
		var response = await _sut.ProcessAsync(invalidWithdrawalRequestId, invalidRequest);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		response.Errors.Should().HaveCount(1);
	}
}
