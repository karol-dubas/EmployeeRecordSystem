using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace EmployeeRecordSystem.IntegrationTests.Helpers;

public class FakePolicyEvaluator : IPolicyEvaluator
{
	public Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
	{
		var result = AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(), "test"));
		return Task.FromResult(result);
	}

	public Task<PolicyAuthorizationResult> AuthorizeAsync(
		AuthorizationPolicy policy,
		AuthenticateResult authenticationResult,
		HttpContext context,
		object resource)
	{
		var result = PolicyAuthorizationResult.Success();
		return Task.FromResult(result);
	}
}
