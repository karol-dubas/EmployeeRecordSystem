using EmployeeRecordSystem.Server.Services;
using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.Shared.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeRecordSystem.Server.Controllers;

[Route("api/employees")]
[ApiController]
[Authorize]
public class EmployeeController : ControllerBase
{
	private readonly IEmployeeService _employeeService;

	public EmployeeController(IEmployeeService employeeService)
	{
		_employeeService = employeeService;
	}

	/// <summary>
	///     Get details with employee billing etc.
	/// </summary>
	/// <remarks>
	///     Authorize: admin or employee's own profile
	/// </remarks>
	[HttpGet("{employeeId:guid}")]
	public IActionResult GetDetails([FromRoute] Guid employeeId)
	{
		var response = _employeeService.GetDetails(employeeId);
		return Ok(response);
	}

	/// <summary>
	///     Get all
	/// </summary>
	/// <remarks>
	///     Authorize: logged user
	/// </remarks>
	[HttpGet]
	public IActionResult GetAll([FromQuery] EmployeeQuery query)
	{
		var response = _employeeService.GetAll(query);
		return Ok(response);
	}

	/// <summary>
	///     Get employee's balance log
	/// </summary>
	/// <remarks>
	///     Authorize: admin or employee's own balance logs
	/// </remarks>
	[HttpGet("{employeeId:guid}/balance-log")]
	public IActionResult GetBalanceLogs([FromRoute] Guid employeeId, [FromQuery] BalanceLogQuery query)
	{
		var response = _employeeService.GetBalanceLogs(employeeId, query);
		return Ok(response);
	}

	/// <summary>
	///     Edit employee's: First Name, Last Name, Bank account number
	/// </summary>
	/// <remarks>
	///     Authorize: admin or employee's own profile
	/// </remarks>
	[HttpPut("{employeeId:guid}")]
	public IActionResult Edit([FromRoute] Guid employeeId, [FromBody] EditEmployeeRequest request)
	{
		_employeeService.Edit(employeeId, request);
		return NoContent();
	}

	/// <summary>
	///     Change employee's hourly pay
	/// </summary>
	/// <remarks>
	///     Authorize: admin
	/// </remarks>
	[HttpPatch("{employeeId:guid}/hourly-pay")]
	[Authorize(Roles = Roles.Admin)]
	public IActionResult ChangeHourlyPay([FromRoute] Guid employeeId, [FromBody] ChangeEmployeeHourlyPayRequest request)
	{
		_employeeService.ChangeHourlyPay(employeeId, request);
		return NoContent();
	}

	/// <summary>
	///     Change the work time of employees
	/// </summary>
	/// <remarks>
	///     Authorize: admin or group supervisor
	/// </remarks>
	[HttpPatch("work-time")]
	[Authorize(Roles = $"{Roles.Admin},{Roles.Supervisor}")]
	public IActionResult ChangeWorkTime([FromBody] ChangeEmployeesWorkTimeRequest request)
	{
		_employeeService.ChangeWorkTimes(request);
		return NoContent();
	}

	/// <summary>
	///     Convert time worked of all employees to the balance
	/// </summary>
	/// <remarks>
	///     Authorize: admin
	/// </remarks>
	[HttpPatch("work-time/convert")]
	[Authorize(Roles = Roles.Admin)]
	public IActionResult ConvertWorkTimeToBalance([FromBody] ConvertTimeRequest request)
	{
		_employeeService.ConvertWorkTimeToBalance(request);
		return NoContent();
	}
}
