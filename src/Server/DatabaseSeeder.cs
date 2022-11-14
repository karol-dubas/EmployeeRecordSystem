using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EmployeeRecordSystem.Server;

public class DatabaseSeeder
{
	private readonly ApplicationDbContext _dbContext;
	private readonly RoleManager<ApplicationRole> _roleManager;
	private readonly UserManager<Employee> _userManager;
	private readonly InitAdminConfiguration _config;
	
	public DatabaseSeeder(
		ApplicationDbContext applicationDbContext,
		UserManager<Employee> userManager,
		RoleManager<ApplicationRole> roleManager,
		IOptions<InitAdminConfiguration> config)
	{
		_dbContext = applicationDbContext;
		_userManager = userManager;
		_roleManager = roleManager;
		_config = config.Value;
	}

	/// <summary>
	///		Will create the database if it does not already exist.
	/// 	Applies any pending migrations for the context to the database.
	/// </summary>
	public DatabaseSeeder Migrate()
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

		bool adminExist = await _userManager.FindByNameAsync(_config.Email) is not null;
		if (!adminExist)
			await SeedAdminAsync();
	}

	private async Task SeedAdminAsync()
	{
		var admin = new Employee
		{
			FirstName = _config.FirstName,
			LastName = _config.LastName,
			Email = _config.Email,
			EmailConfirmed = true,
			UserName = _config.Email
		};

		await _userManager.CreateAsync(admin, _config.Password);
		await _userManager.AddToRoleAsync(admin, Roles.Admin);
	}

	private async Task<bool> RoleExistsAsync(string roleName)
	{
		return await _roleManager.FindByNameAsync(roleName) is not null;
	}
}
