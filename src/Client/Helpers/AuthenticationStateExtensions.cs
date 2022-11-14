using Microsoft.AspNetCore.Components.Authorization;

namespace EmployeeRecordSystem.Client.Helpers;

public static class AuthenticationStateExtensions
{
	public static Guid GetUserId(this AuthenticationState auth)
	{
		string userId = auth.User.FindFirst(c => c.Type == "sub")?.Value;
		return userId is null ? default : Guid.Parse(userId);
	}
}
