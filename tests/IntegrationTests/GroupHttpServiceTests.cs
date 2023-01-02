using System.Net;
using EmployeeRecordSystem.Client.HttpServices;
using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.IntegrationTests.Helpers;
using EmployeeRecordSystem.Server;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.Shared.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Build.Graph;
using Microsoft.EntityFrameworkCore;
using Moq;
using Program = EmployeeRecordSystem.Server.Program;

namespace EmployeeRecordSystem.IntegrationTests;

public class GroupHttpServiceTests : IntegrationTest
{
	private readonly GroupHttpService _sut;

	public GroupHttpServiceTests()
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
		_sut = new GroupHttpService(httpClient);

		DbContext = GetDbContext(factory);
	}

	[Fact]
	public async Task CreateAsync_ForValidRequest_ReturnsCreatedWithCreatedGroup()
	{
		// Arrange
		var request = new CreateGroupRequest { Name = "new group" };

		// Act
		var response = await _sut.CreateAsync(request);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.Created);
		response.DeserializedContent.Should().NotBeNull();
	}

	[Fact]
	public async Task CreateAsync_ForInvalidRequest_ReturnsBadRequest()
	{
		// Arrange
		var request = new CreateGroupRequest { Name = "" };

		// Act
		var response = await _sut.CreateAsync(request);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		response.Errors.Should().HaveCount(1);
	}

	[Fact]
	public async Task GetAllAsync_ForValidQuery_ReturnsOkWithGroups()
	{
		// Arrange
		var group = SeedGroup();
		var query = new GroupQuery { Id = group.Id };

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
		var invalidQuery = new GroupQuery { Id = invalidGroupId };

		// Act
		var response = await _sut.GetAllAsync(invalidQuery);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		response.Errors.Should().HaveCount(1);
	}

	[Fact]
	public async Task RenameAsync_ForValidInput_ReturnsOkWithRenamedGroup()
	{
		// Arrange
		var group = SeedGroup();
		const string newGroupName = "renamed";
		var request = new RenameGroupRequest { Name = newGroupName };

		// Act
		var response = await _sut.RenameAsync(group.Id, request);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		response.DeserializedContent.Name.Should().Be(newGroupName);
	}

	[Fact]
	public async Task RenameAsync_ForInvalidId_ReturnsNotFound()
	{
		// Arrange
		var invalidGroupId = Guid.NewGuid();
		var request = new RenameGroupRequest { Name = "renamed" };

		// Act
		var response = await _sut.RenameAsync(invalidGroupId, request);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		response.Errors.Should().HaveCount(1);
	}

	[Fact]
	public async Task RenameAsync_ForInvalidRequest_ReturnsBadRequest()
	{
		// Arrange
		var invalidRequest = new RenameGroupRequest { Name = "" };

		// Act
		var response = await _sut.RenameAsync(It.IsAny<Guid>(), invalidRequest);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		response.Errors.Should().HaveCount(1);
	}

	[Fact]
	public async Task AssignEmployeeToGroupAsync_ForValidIds_ReturnsNoContent()
	{
		// Arrange
		var employee = SeedEmployee();
		var group = SeedGroup();

		// Act
		var response = await _sut.AssignEmployeeToGroupAsync(group.Id, employee.Id);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task AssignEmployeeToGroupAsync_ForInvalidIds_ReturnsNotFound()
	{
		// Arrange
		var invalidEmployee = Guid.NewGuid();
		var invalidGroupId = Guid.NewGuid();

		// Act
		var response = await _sut.AssignEmployeeToGroupAsync(invalidGroupId, invalidEmployee);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		response.Errors.Should().HaveCount(1);
	}

	[Fact]
	public async Task RemoveEmployeeFromGroupAsync_ForValidId_ReturnsNoContent()
	{
		// Arrange
		var employee = SeedEmployee();
		var group = SeedGroup();
		AddEmployeeToGroup(employee, group);

		// Act
		var response = await _sut.RemoveEmployeeFromGroupAsync(employee.Id);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task RemoveEmployeeFromGroupAsync_ForInvalidInput_ReturnsNotFound()
	{
		// Arrange
		var invalidEmployeeId = Guid.NewGuid();

		// Act
		var response = await _sut.RemoveEmployeeFromGroupAsync(invalidEmployeeId);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		response.Errors.Should().HaveCount(1);
	}

	[Fact]
	public async Task DeleteAsync_ForValidId_ReturnsNoContent()
	{
		// Arrange
		var group = SeedGroup();

		// Act
		var response = await _sut.DeleteAsync(group.Id);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task DeleteAsync_ForInvalidId_ReturnsNotFound()
	{
		// Arrange
		var invalidGroupId = Guid.NewGuid();

		// Act
		var response = await _sut.DeleteAsync(invalidGroupId);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		response.Errors.Should().HaveCount(1);
	}
}
