using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRecordSystem.Data.Helpers;

public class DatabaseSeeder
{
	private const string _adminEmail = "admin@admin.com";
	private readonly ApplicationDbContext _dbContext;
	private readonly RoleManager<ApplicationRole> _roleManager;
	private readonly UserManager<Employee> _userManager;

	public DatabaseSeeder(
		ApplicationDbContext applicationDbContext,
		UserManager<Employee> userManager,
		RoleManager<ApplicationRole> roleManager)
	{
		_dbContext = applicationDbContext;
		_userManager = userManager;
		_roleManager = roleManager;
	}

	/// <summary>
	///     Create the database and migrate if it doesn't exist
	/// </summary>
	public DatabaseSeeder EnsureDatabaseCreated()
	{
		if (!_dbContext.Database.CanConnect())
			_dbContext.Database.Migrate();

		return this;
	}

	public DatabaseSeeder ApplyPendingMigrations()
	{
		bool isDatabaseForTests = !_dbContext.Database.IsRelational();
		if (isDatabaseForTests)
			return this;

		var pendingMigrations = _dbContext.Database.GetPendingMigrations();
		if (pendingMigrations.Any())
			_dbContext.Database.Migrate();

		return this;
	}

	public async Task SeedAsync()
	{
		if (!await RoleExistsAsync(Roles.Admin))
			await _roleManager.CreateAsync(new ApplicationRole(Roles.Admin));

		if (!await RoleExistsAsync(Roles.Supervisor))
			await _roleManager.CreateAsync(new ApplicationRole(Roles.Supervisor));

		if (!await RoleExistsAsync(Roles.Employee))
			await _roleManager.CreateAsync(new ApplicationRole(Roles.Employee));

		bool adminExist = await _userManager.FindByNameAsync(_adminEmail) is not null;
		if (!adminExist)
			await SeedAdminAsync();
	}

	private async Task SeedAdminAsync()
	{
		var admin = new Employee
		{
			FirstName = "Admin",
			LastName = "Admin",
			Email = _adminEmail,
			EmailConfirmed = true,
			UserName = _adminEmail
		};

		const string password = "Admin1234!";

		await _userManager.CreateAsync(admin, password);
		await _userManager.AddToRoleAsync(admin, Roles.Admin);
	}

	private async Task<bool> RoleExistsAsync(string roleName)
	{
		return await _roleManager.FindByNameAsync(roleName) is not null;
	}
}
