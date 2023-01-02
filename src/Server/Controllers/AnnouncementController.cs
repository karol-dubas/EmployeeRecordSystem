using EmployeeRecordSystem.Server.Services;
using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.Shared.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeRecordSystem.Server.Controllers;

[Route("api/announcements")]
[ApiController]
[Authorize]
public class AnnouncementController : ControllerBase
{
	private readonly IAnnouncementService _announcementService;

	public AnnouncementController(IAnnouncementService announcementService)
	{
		_announcementService = announcementService;
	}

	/// <summary>
	///     Create
	/// </summary>
	/// <remarks>
	///     Authorize: admin and supervisor
	/// </remarks>
	[HttpPost]
	[Authorize(Roles = $"{Roles.Admin},{Roles.Supervisor}")]
	public IActionResult Create([FromBody] CreateAnnouncementRequest request)
	{
		var response = _announcementService.Create(request);
		return CreatedAtAction(nameof(GetAll), new { response.Id }, response);
	}

	/// <summary>
	///     Get all
	/// </summary>
	/// <remarks>
	///     Authorize: logged employee
	/// </remarks>
	[HttpGet]
	public IActionResult GetAll([FromQuery] AnnouncementQuery query)
	{
		var response = _announcementService.GetAll(query);
		return Ok(response);
	}

	/// <summary>
	///     Update
	/// </summary>
	/// <remarks>
	///     Authorize: admin and supervisor
	/// </remarks>
	[HttpPut("{announcementId:guid}")]
	[Authorize(Roles = $"{Roles.Admin},{Roles.Supervisor}")]
	public IActionResult Update([FromRoute] Guid announcementId, [FromBody] CreateAnnouncementRequest request)
	{
		_announcementService.Update(announcementId, request);
		return NoContent();
	}

	/// <summary>
	///     Delete
	/// </summary>
	/// <remarks>
	///     Authorize: admin and supervisor
	/// </remarks>
	[HttpDelete("{announcementId:guid}")]
	[Authorize(Roles = $"{Roles.Admin},{Roles.Supervisor}")]
	public IActionResult Delete([FromRoute] Guid announcementId)
	{
		_announcementService.Delete(announcementId);
		return NoContent();
	}
}
