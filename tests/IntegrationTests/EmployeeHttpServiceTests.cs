using System.Net;
using EmployeeRecordSystem.Client.HttpServices;
using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.IntegrationTests.Helpers;
using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.Shared.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using Program = EmployeeRecordSystem.Server.Program;

namespace EmployeeRecordSystem.IntegrationTests;

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
	public async Task GetDetailsAsync_ForValidId_ReturnsOkWithEmployeeDetails()
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
	public async Task GetDetailsAsync_ForInvalidId_ReturnsNotFound()
	{
	    // Arrange
	    var invalidEmployeeId = Guid.NewGuid();
	    
	    // Act
	    var response = await _sut.GetDetailsAsync(invalidEmployeeId);

	    // Assert
	    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	    response.Errors.Should().HaveCount(1);
	}
	
	[Fact]
	public async Task GetAllAsync_ForValidQuery_ReturnsOkWithEmployees()
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
	public async Task GetAllAsync_ForInvalidQuery_ReturnsBadRequest()
	{
		// Arrange
		var invalidGroupId = Guid.NewGuid();
		var query = new EmployeeQuery() { GroupId = invalidGroupId };
		
		// Act
		var response = await _sut.GetAllAsync(query);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		response.Errors.Should().HaveCount(1);
	}
	
	[Fact]
	public async Task GetBalanceLogAsync_ForValidInput_ReturnsOkWithBalanceLogs()
	{
	    // Arrange
	    var employee = SeedEmployee();
	    SeedBalanceLog(employee);
	    var query = new BalanceLogQuery()
	    {
		    PageNumber = 1,
		    PageSize = 1,
	    };
	    
	    // Act
	    var response = await _sut.GetBalanceLogsAsync(employee.Id, query);

	    // Assert
	    response.StatusCode.Should().Be(HttpStatusCode.OK);
	    response.DeserializedContent.Items.Should().HaveCount(1);
	}

	[Fact]
	public async Task GetBalanceLogAsync_ForInvalidId_ReturnsNotFound()
	{
		// Arrange
		var invalidEmployeeId = Guid.NewGuid();
		var query = new BalanceLogQuery()
		{
			PageNumber = 1,
			PageSize = 1,
		};
		
		// Act
		var response = await _sut.GetBalanceLogsAsync(invalidEmployeeId, query);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		response.Errors.Should().HaveCount(1);
	}

	[Fact]
	public async Task GetBalanceLogAsync_ForInvalidQuery_ReturnsBadRequest()
	{
		// Arrange
		var employee = SeedEmployee();
		var invalidQuery = new BalanceLogQuery()
		{
			SortBy = "invalid",
			SortDirection = "invalid",
			PageSize = -1,
			PageNumber = -1,
		};
		
		// Act
		var response = await _sut.GetBalanceLogsAsync(employee.Id, invalidQuery);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		response.Errors.Should().HaveCount(4);
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
	public async Task EditAsync_ForInvalidId_ReturnsNotFound()
	{
		// Arrange
		var invalidEmployeeId = Guid.NewGuid();
		var validRequest = new EditEmployeeRequest
		{
			FirstName = "renamed",
			LastName = "renamed"
		};
	    
		// Act
		var response = await _sut.EditAsync(invalidEmployeeId, validRequest);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		response.Errors.Should().HaveCount(1);
	}

	[Fact]
	public async Task EditAsync_ForInvalidRequest_ReturnsBadRequest()
	{
		// Arrange
		var invalidRequest = new EditEmployeeRequest
		{
			FirstName = "",
			LastName = "",
			BankAccountNumber = RandomString.Generate(35),
			Note = RandomString.Generate(301)
		};

		// Act
		var response = await _sut.EditAsync(It.IsAny<Guid>(), invalidRequest);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		response.Errors.Should().HaveCount(4);
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
	public async Task ChangeHourlyPayAsync_ForInvalidId_ReturnsNotFound()
	{
		// Arrange
		var invalidEmployeeId = Guid.NewGuid();
		var request = new ChangeEmployeeHourlyPayRequest() { HourlyPay = 1 };

		// Act
		var response = await _sut.ChangeHourlyPayAsync(invalidEmployeeId, request);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		response.Errors.Should().HaveCount(1);
	}

	[Fact]
	public async Task ChangeHourlyPayAsync_ForInvalidRequest_ReturnsBadRequest()
	{
		// Arrange
		var invalidRequest = new ChangeEmployeeHourlyPayRequest() { HourlyPay = -1 };

		// Act
		var response = await _sut.ChangeHourlyPayAsync(It.IsAny<Guid>(), invalidRequest);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		response.Errors.Should().HaveCount(1);
	}

	[Fact]
	public async Task ChangeWorkTimeAsync_ForValidRequest_ReturnsNoContent()
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
	public async Task ConvertWorkTimeToBalanceAsync_ForValidRequest_ReturnsNoContent()
	{
		// Arrange
		var employee = SeedEmployee();
		SeedEmployeeBilling(employee);
		var group = SeedGroup();
		AddEmployeeToGroup(employee, group);
		var validRequest = new ConvertTimeRequest { GroupId = group.Id };

		// Act
		var response = await _sut.ConvertWorkTimeToBalanceAsync(validRequest);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task ConvertWorkTimeToBalanceAsync_ForInvalidRequest_ReturnsBadRequest()
	{
		// Arrange
		var invalidGroupId = Guid.NewGuid();
		var validRequest = new ConvertTimeRequest { GroupId = invalidGroupId };

		// Act
		var response = await _sut.ConvertWorkTimeToBalanceAsync(validRequest);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		response.Errors.Should().HaveCount(1);
	}
}
