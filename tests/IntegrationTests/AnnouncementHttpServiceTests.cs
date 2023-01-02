using System.Net;
using EmployeeRecordSystem.Client.HttpServices;
using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.IntegrationTests.Helpers;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.Shared.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Program = EmployeeRecordSystem.Server.Program;

namespace EmployeeRecordSystem.IntegrationTests;

public class AnnouncementHttpServiceTests : IntegrationTest
{
	private readonly AnnouncementHttpService _sut;
	
	public AnnouncementHttpServiceTests()
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
		_sut = new AnnouncementHttpService(httpClient);

		DbContext = GetDbContext(factory);
	}
	
	
	[Fact]
	public async Task CreateAsync_ForValidRequest_ReturnsCreatedWithCreatedAnnouncement()
	{
		// Arrange
		var request = new CreateAnnouncementRequest
		{
			Title = "Test",
			Text = ""
		};

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
		var request = new CreateAnnouncementRequest();

		// Act
		var response = await _sut.CreateAsync(request);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		response.Errors.Should().HaveCount(1);
	}

	[Fact]
	public async Task GetAllAsync_ForExistingAnnouncements_ReturnsOkAndAnnouncements()
	{
		// Arrange
		var employee = SeedEmployee();
		var announcement = SeedAnnouncement(employee.Id);
		SeedAnnouncement(Guid.NewGuid());
		
		var query = new AnnouncementQuery
		{
			Id = announcement.Id
		};

		// Act
		var response = await _sut.GetAllAsync(query);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		response.DeserializedContent.Should().HaveCount(1);
	}

	[Fact]
	public async Task UpdateAsync_ForValidInput_ReturnsNoContent()
	{
		// Arrange
		var announcement = SeedAnnouncement(Guid.NewGuid());
		var request = new CreateAnnouncementRequest
		{
			Title = "rename test"
		};
		
		// Act
		var response = await _sut.UpdateAsync(announcement.Id, request);
		
		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task UpdateAsync_ForInvalidInput_ReturnsBadRequest()
	{
		// Arrange
		var request = new CreateAnnouncementRequest
		{
			Title = ""
		};

		// Act
		var response = await _sut.UpdateAsync(Guid.NewGuid(), request);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		response.Errors.Should().HaveCount(1);
	}

	[Fact]
	public async Task DeleteAsync_ForValidId_ReturnsNoContent()
	{
		// Arrange
		var announcement = SeedAnnouncement(Guid.NewGuid());

		// Act
		var response = await _sut.DeleteAsync(announcement.Id);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task DeleteAsync_ForInvalidId_ReturnsNotFound()
	{
		// Arrange
		var invalidAnnouncementId = Guid.NewGuid();

		// Act
		var response = await _sut.DeleteAsync(invalidAnnouncementId);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		response.Errors.Should().HaveCount(1);
	}
}
