using System.Security.Claims;
using EmployeeRecordSystem.Shared.Constants;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmployeeRecordSystem.Tests.Helpers;

public class FakeAdminClaimsFilter : IAsyncActionFilter
{
	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		var cp = new ClaimsPrincipal();
		cp.AddIdentity(new ClaimsIdentity(new []
		{
			new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
			new Claim(ClaimTypes.Role, Roles.Admin)
		}));

		context.HttpContext.User = cp;

		await next();
	}
}
