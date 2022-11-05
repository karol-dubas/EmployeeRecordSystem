using System.Net;
using EmployeeRecordSystem.Client.Helpers;
using EmployeeRecordSystem.Client.HttpServices;
using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Data.Helpers;
using EmployeeRecordSystem.IntegrationTests.Helpers;
using EmployeeRecordSystem.Server;
using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Program = EmployeeRecordSystem.Server.Program;

namespace EmployeeRecordSystem.IntegrationTests;

public class RoleHttpServiceTests : IntegrationTest
{
	private readonly RoleHttpService _sut;

	public RoleHttpServiceTests()
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
		_sut = new RoleHttpService(httpClient);
		
		DbContext = GetDbContext(factory);
	}

	[Fact]
	public async Task GetAllAsync_WithRoles_ReturnsOkWithThreeRoles()
	{
		// Arrange

		// Act
		var response = await _sut.GetAllAsync();

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		response.DeserializedContent.Should().HaveCount(3);
	}

	[Fact]
	public async Task ChangeEmployeeRoleAsync_ForInvalidIds_ReturnsNotFoundStatusCode()
	{
		// Arrange
		var invalidEmployeeId = Guid.NewGuid();
		var invalidNewRoleId = Guid.NewGuid();

		// Act
		var response = await _sut.ChangeEmployeeRoleAsync(invalidEmployeeId, invalidNewRoleId);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
	
	[Fact]
	public async Task ChangeEmployeeRoleAsync_ForValidIds_ReturnsNoContentStatusCode()
	{
		// Arrange
		var employee = SeedEmployee();
		
		var managerRole = GetRole(Roles.Supervisor);
		var employeeRole = GetRole(Roles.Employee);

		AddEmployeeToRole(employee, employeeRole);

		// Act
		var response = await _sut.ChangeEmployeeRoleAsync(employee.Id, managerRole.Id);
		
		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}
}
