using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Client.Helpers;

public static class AuthenticationStateProviderExtensions
{
    public static async Task<Guid> GetUserIdAsync(this AuthenticationStateProvider authenticationStateProvider)
    {
        var user = (await authenticationStateProvider.GetAuthenticationStateAsync()).User;

        if (!user.Identity.IsAuthenticated)
            return default;
            
        string userId =  user.FindFirst(c => c.Type == "sub")?.Value;
        return Guid.Parse(userId);
    }

    public static async Task<bool> IsUserInRole(this AuthenticationStateProvider authenticationStateProvider, string role)
    {
        var user = (await authenticationStateProvider.GetAuthenticationStateAsync()).User;

        if (!user.Identity.IsAuthenticated)
            return false;
            
        string claimsRole = user.FindFirst(c => c.Type == "role")?.Value;
        return claimsRole == role;
    }
}