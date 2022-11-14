using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EmployeeRecordSystem.IntegrationTests.Helpers;

public class IntegrationTest
{
	protected ApplicationDbContext DbContext;
	
	protected void AddEmployeeToRole(Employee employee, ApplicationRole role)
	{
		DbContext.UserRoles.Add(new IdentityUserRole<Guid>()
		{
			RoleId = role.Id,
			UserId = employee.Id
		});

		DbContext.SaveChanges();
	}
	
	protected void AddEmployeeToGroup(Employee employee, Group group)
	{
		group.Employees.Add(employee);
		DbContext.SaveChanges();
	}

	protected ApplicationRole GetRole(string role)
	{
		return DbContext.Roles.Single(r => r.Name == role);
	}
	
	protected Employee SeedEmployee()
	{
		var newEmployee = new Employee()
		{
			FirstName = "test",
			LastName = "test",
			UserName = "test@test.com",
			PasswordHash = "1",
			SecurityStamp = "1"
		};
			
		DbContext.Users.Add(newEmployee);
		DbContext.SaveChanges();
		
		return newEmployee;
	}
	
	protected void SeedEmployeeBilling(Employee employee)
	{
		employee.EmployeeBilling.HourlyPay = 10;
		employee.EmployeeBilling.TimeWorked = TimeSpan.FromHours(1);

		DbContext.SaveChanges();
	}
	
	protected void SeedBalanceLog(Employee employee)
	{
		var log = new BalanceLog()
		{
			EmployeeId = employee.Id,
			CreatedAt = DateTimeOffset.Now
		};
		
		employee.BalanceLogs.Add(log);

		DbContext.SaveChanges();
	}
	
	protected Group SeedGroup()
	{
		var group = new Group
		{
			Name = "test group"
		};
		
		DbContext.Groups.Add(group);
		DbContext.SaveChanges();
		
		return group;
	}
	
	protected WithdrawalRequest SeedWithdrawalRequest(Guid employeeId)
	{
		var withdrawalRequest = new WithdrawalRequest()
		{
			WithdrawalRequestStatusTypeCode = WithdrawalRequestStatusType.Pending.Code,
			EmployeeId = employeeId
		};

		DbContext.WithdrawalRequests.Add(withdrawalRequest);
		DbContext.SaveChanges();
		
		return withdrawalRequest;
	}
	
	protected static ApplicationDbContext GetDbContext(WebApplicationFactory<Program> factory)
	{
		var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();
		/* using */ var scope = scopeFactory.CreateScope();
		return scope.ServiceProvider.GetService<ApplicationDbContext>();
	}
}
