using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Data.Helpers
{
    public class DatabaseSeeder
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<Employee> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public DatabaseSeeder(ApplicationDbContext applicationDbContext,
            UserManager<Employee> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _dbContext = applicationDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Create the database and migrate if it doesn't exist
        /// </summary>
        public DatabaseSeeder EnsureDatabaseCreated()
        {
            if (!_dbContext.Database.CanConnect())
            {
                RelationalDatabaseFacadeExtensions.Migrate(_dbContext.Database);
            }

            return this;
        }

        public DatabaseSeeder ApplyPendingMigrations()
        {
            bool isTestDb = !_dbContext.Database.IsRelational();
            if (isTestDb)
                return this;

            var pendingMigrations = _dbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                _dbContext.Database.Migrate();
            }

            return this;
        }

        public async Task SeedAsync()
        {
            if (!await RoleExistsAsync(Roles.Admin))
                await _roleManager.CreateAsync(new(Roles.Admin));

            if (!await RoleExistsAsync(Roles.Supervisor))
                await _roleManager.CreateAsync(new(Roles.Supervisor));

            if (!await RoleExistsAsync(Roles.Employee))
                await _roleManager.CreateAsync(new(Roles.Employee));

            bool adminExist = await _userManager.FindByNameAsync("admin@admin.com") is not null;
            if (!adminExist)
                await SeedAdminAsync();
        }

        private async Task SeedAdminAsync()
        {
            var admin = new Employee
            {
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@admin.com",
                EmailConfirmed = true,
                UserName = "admin@admin.com",
            };

            var password = "Admin1234!";

            await _userManager.CreateAsync(admin, password);
            await _userManager.AddToRoleAsync(admin, Roles.Admin);
        }

        private async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName) is not null;
        }
    }
}
