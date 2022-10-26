using EmployeeRecordSystem.Server.Services;
using EmployeeRecordSystem.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeRecordSystem.Server.Controllers;

[Route("api/roles")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    ///     Get all
    /// </summary>
    /// <remarks>
    ///     Authorize: admin
    /// </remarks>
    [HttpGet]
    public IActionResult GetAll()
    {
        var response = _roleService.GetAll();
        return Ok(response);
    }

    /// <summary>
    ///     Change employee's role
    /// </summary>
    /// <remarks>
    ///     Authorize: admin
    /// </remarks>
    [HttpPatch("{newRoleId}/employee/{employeeId}")]
    public IActionResult ChangeEmployeeRole(Guid employeeId, Guid newRoleId)
    {
        _roleService.ChangeEmployeeRole(employeeId, newRoleId);
        return NoContent();
    }
}