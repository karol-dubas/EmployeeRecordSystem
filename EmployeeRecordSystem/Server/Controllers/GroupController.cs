using EmployeeRecordSystem.Server.Services;
using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.Shared.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeRecordSystem.Server.Controllers;

[Route("api/groups")]
[ApiController]
[Authorize]
public class GroupController : ControllerBase
{
	private readonly IGroupService _groupService;

	public GroupController(IGroupService groupService)
	{
		_groupService = groupService;
	}

    /// <summary>
    ///     Create
    /// </summary>
    /// <remarks>
    ///     Authorize: admin
    /// </remarks>
    [HttpPost]
	[Authorize(Roles = Roles.Admin)]
	public IActionResult Create([FromBody] CreateGroupRequest request)
	{
		var response = _groupService.Create(request);
		return CreatedAtAction(nameof(GetAll), new { response.Id }, response);
	}

    /// <summary>
    ///     Get all groups with employees
    /// </summary>
    /// <remarks>
    ///     Authorize: logged user
    /// </remarks>
    [HttpGet]
	public IActionResult GetAll([FromQuery] GroupQuery query)
	{
		var response = _groupService.GetAll(query);
		return Ok(response);
	}

    /// <summary>
    ///     Rename
    /// </summary>
    /// <remarks>
    ///     Authorize: admin
    /// </remarks>
    [HttpPatch("{groupId}")]
	[Authorize(Roles = Roles.Admin)]
	public IActionResult Rename([FromRoute] Guid groupId, [FromBody] RenameGroupRequest request)
	{
		var response = _groupService.Rename(groupId, request);
		return Ok(response);
	}

    /// <summary>
    ///     Assign employee to a group
    /// </summary>
    /// <remarks>
    ///     Authorize: admin
    /// </remarks>
    [HttpPatch("{groupId}/employee/{employeeId}")]
	[Authorize(Roles = Roles.Admin)]
	public IActionResult AssignEmployeeToGroup([FromRoute] Guid groupId, [FromRoute] Guid employeeId)
	{
		_groupService.AssignEmployeeToGroup(groupId, employeeId);
		return NoContent();
	}

    /// <summary>
    ///     Remove employee from the group
    /// </summary>
    /// <remarks>
    ///     Authorize: admin
    /// </remarks>
    [HttpDelete("employee/{employeeId}")]
	[Authorize(Roles = Roles.Admin)]
	public IActionResult RemoveEmployeeFromGroup([FromRoute] Guid employeeId)
	{
		_groupService.RemoveEmployeeFromGroup(employeeId);
		return NoContent();
	}

    /// <summary>
    ///     Delete
    /// </summary>
    /// <remarks>
    ///     Authorize: admin
    /// </remarks>
    [HttpDelete("{groupId}")]
	[Authorize(Roles = Roles.Admin)]
	public IActionResult Delete([FromRoute] Guid groupId)
	{
		_groupService.Delete(groupId);
		return NoContent();
	}
}
