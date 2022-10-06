using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using EmployeeRecordSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using Microsoft.AspNetCore.Hosting;
using EmployeeRecordSystem.Server.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using EmployeeRecordSystem.Data;

namespace EmployeeRecordSystem.Tests
{
    public class ProgramTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly List<Type> _controllerTypes;
        private readonly WebApplicationFactory<Program> _factory;

        public ProgramTests(WebApplicationFactory<Program> factory)
        {
            _controllerTypes = typeof(Program)
            .Assembly
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(ControllerBase)))
            .ToList();

            _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.Single(service => service.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    services.Remove(dbContextOptions);
                    services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("RestaurantDb"));

                    _controllerTypes.ForEach(c => services.AddScoped(c));
                });
            });
        }

        [Fact]
        public void RegisterServices_ForAllControllers_RegistersAllDependencies()
        {
            // Arrange
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();

            // Act

            // Assert
            _controllerTypes.ForEach(c =>
            {
                var controller = scope.ServiceProvider.GetService(c);
                controller.Should().NotBeNull();
            });
        }
    }
}