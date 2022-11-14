using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EmployeeRecordSystem.Shared.Constants;
using static EmployeeRecordSystem.Server.Installers.Helpers.ServiceAttributes;

namespace EmployeeRecordSystem.Server.Services;

public interface IAuthorizationService
{
    bool IsAdmin();
    bool IsUserOwnResource(Guid employeeId);
    bool IsSupervisor(); 
    Guid? UserId { get; }
}

[ScopedRegistration]
public class AuthorizationService : IAuthorizationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

    public Guid? UserId => User is null ? 
        null : 
        Guid.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

    public bool IsAdmin()
    {
        return User.IsInRole(Roles.Admin);
    }

    public bool IsUserOwnResource(Guid employeeId)
    {
        return UserId == employeeId;
    }

    public bool IsSupervisor()
    {
        return User.IsInRole(Roles.Supervisor);
    }
}