using EmployeeRecordSystem.Client.HttpServices;
using EmployeeRecordSystem.Data;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Tests
{
    public class RoleHttpServiceTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly RoleHttpService _httpService;

        public RoleHttpServiceTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services.Single(service => service.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                        services.Remove(dbContextOptions);
                        services.AddDbContext<ApplicationDbContext>(o => o.UseInMemoryDatabase("testDb"));
                    });
                });

            _client = _factory.CreateClient();
            _httpService = new RoleHttpService(_client);
        }

        [Fact]
        public async Task GetAllAsync_WithRoles_ReturnsThreeRoles()
        {
            // Arrange

            // Act
            var response = await _httpService.GetAllAsync();

            // Assert
            response.Should().HaveCount(3);
        }
    }
}
