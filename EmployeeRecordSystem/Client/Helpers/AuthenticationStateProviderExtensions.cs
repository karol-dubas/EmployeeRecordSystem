using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Client.Helpers
{
    public static class AuthenticationStateProviderExtensions
    {
        public static async Task<Guid> GetUserIdAsync(this AuthenticationStateProvider authenticationStateProvider)
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var userId =  user.FindFirst(c => c.Type == "sub")?.Value;
                return Guid.Parse(userId);
            }

            return default;
        }
    }
}
