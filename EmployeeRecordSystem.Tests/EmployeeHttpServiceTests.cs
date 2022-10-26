using System.Net;
using EmployeeRecordSystem.Client.HttpServices;
using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Server;
using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.Shared.Requests;
using EmployeeRecordSystem.Tests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRecordSystem.Tests;

public class EmployeeHttpServiceTests : IntegrationTest
{
	private readonly EmployeeHttpService _sut;

	public EmployeeHttpServiceTests()
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
		_sut = new EmployeeHttpService(httpClient);

		DbContext = GetDbContext(factory);
	}
	
	[Fact]
	public async Task GetDetailsAsync_ForValidInput_ReturnsOkWithEmployeeDetails()
	{
	    // Arrange
	    var employee = SeedEmployee();
	    var employeeRole = GetRole(Roles.Employee);
	    AddEmployeeToRole(employee, employeeRole);
	    
	    // Act
	    var response = await _sut.GetDetailsAsync(employee.Id);

	    // Assert
	    response.StatusCode.Should().Be(HttpStatusCode.OK);
	    response.DeserializedContent.Should().NotBeNull();
	}
	
	[Fact]
	public async Task GetDetailsAsync_ForInvalidInput_ReturnsNotFound()
	{
	    // Arrange
	    var invalidEmployeeId = Guid.NewGuid();
	    
	    // Act
	    var response = await _sut.GetDetailsAsync(invalidEmployeeId);

	    // Assert
	    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
	
	[Fact]
	public async Task GetAllAsync_ForValidInput_ReturnsOkWithEmployees()
	{
	    // Arrange
	    var employee = SeedEmployee();
	    var employeeRole = GetRole(Roles.Employee);
	    AddEmployeeToRole(employee, employeeRole);
	    var group = SeedGroup();
	    AddEmployeeToGroup(employee, group);
	    var query = new EmployeeQuery() { GroupId = group.Id };
	    
	    // Act
	    var response = await _sut.GetAllAsync(query);

	    // Assert
	    response.StatusCode.Should().Be(HttpStatusCode.OK);
	    response.DeserializedContent.Should().HaveCount(1);
	}
	
	[Fact]
	public async Task GetAllAsync_ForInvalidInput_ReturnsNotFound()
	{
		// Arrange
		var invalidGroupId = Guid.NewGuid();
		var query = new EmployeeQuery() { GroupId = invalidGroupId };
		
		// Act
		var response = await _sut.GetAllAsync(query);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
	
	[Fact]
	public async Task GetBalanceLogAsync_ForValidInput_ReturnsOkWithBalanceLogs()
	{
	    // Arrange
	    var employee = SeedEmployee();
	    SeedBalanceLog(employee);
	    
	    // Act
	    var response = await _sut.GetBalanceLogAsync(employee.Id);

	    // Assert
	    response.StatusCode.Should().Be(HttpStatusCode.OK);
	    response.DeserializedContent.Should().HaveCount(1);
	}

	[Fact]
	public async Task GetBalanceLogAsync_ForInvalid_ReturnsNotFound()
	{
		// Arrange
		var invalidEmployeeId = Guid.NewGuid();
		
		// Act
		var response = await _sut.GetBalanceLogAsync(invalidEmployeeId);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
	
	[Fact]
	public async Task EditAsync_ForValidInput_ReturnsNoContent()
	{
	    // Arrange
	    var employee = SeedEmployee();
	    var request = new EditEmployeeRequest
	    {
		    FirstName = "renamed",
		    LastName = "renamed"
	    };
	    
	    // Act
	    var response = await _sut.EditAsync(employee.Id, request);

	    // Assert
	    response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}
	
	[Fact]
	public async Task EditAsync_ForInvalidInput_ReturnsNotFound()
	{
		// Arrange
		var invalidEmployeeId = Guid.NewGuid();
		var request = new EditEmployeeRequest
		{
			FirstName = "renamed",
			LastName = "renamed"
		};
	    
		// Act
		var response = await _sut.EditAsync(invalidEmployeeId, request);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task ChangeHourlyPayAsync_ForValidInput_ReturnsNoContent()
	{
		// Arrange
		var employee = SeedEmployee();

		// Act
		var response = await _sut.ChangeHourlyPayAsync(
			employee.Id, new ChangeEmployeeHourlyPayRequest { HourlyPay = 20 });

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task ChangeHourlyPayAsync_ForInvalidInput_ReturnsNotFound()
	{
		// Arrange
		var invalidEmployeeId = Guid.NewGuid();

		// Act
		var response = await _sut.ChangeHourlyPayAsync(
			invalidEmployeeId, new ChangeEmployeeHourlyPayRequest());

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task ChangeWorkTimeAsync_ForValidInput_ReturnsNoContent()
	{
		// Arrange
		var employee = SeedEmployee();
		var request = new ChangeEmployeesWorkTimeRequest
		{
			EmployeeIds = new List<Guid> { employee.Id },
			WorkTimeOperation = WorkTimeOperations.Add,
			WorkTime = TimeSpan.FromHours(1)
		};

		// Act
		var response = await _sut.ChangeWorkTimeAsync(request);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task ConvertWorkTimeToBalanceAsync_ForValidInput_ReturnsNoContent()
	{
		// Arrange
		var employee = SeedEmployee();
		SeedEmployeeBilling(employee);
		var group = SeedGroup();
		AddEmployeeToGroup(employee, group);

		// Act
		var response = await _sut.ConvertWorkTimeToBalanceAsync(new ConvertTimeRequest { GroupId = group.Id });

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}
}
