using EmployeeRecordSystem.Server.Services;
using EmployeeRecordSystem.Shared.Constants;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.Shared.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeRecordSystem.Server.Controllers;

[Route("api/withdrawal-requests")]
[ApiController]
[Authorize]
public class WithdrawalRequestController : ControllerBase
{
    private readonly IWithdrawalRequestService _withdrawalRequestService;

    public WithdrawalRequestController(IWithdrawalRequestService withdrawalRequestService)
    {
        _withdrawalRequestService = withdrawalRequestService;
    }

    /// <summary>
    ///     Create
    /// </summary>
    /// <remarks>
    ///     Authorize: logged user
    /// </remarks>
    [HttpPost("{employeeId}")]
    public IActionResult Create([FromRoute] Guid employeeId, [FromBody] CreateWithdrawalRequestRequest request)
    {
        var response = _withdrawalRequestService.Create(employeeId, request);
        return CreatedAtAction(nameof(GetAll), new { response.Id }, response);
    }

    /// <summary>
    ///     Get all
    /// </summary>
    /// <remarks>
    ///     Authorize: admin or employee's own requests
    /// </remarks>
    [HttpGet]
    public IActionResult GetAll([FromQuery] WithdrawalRequestQuery query)
    {
        var response = _withdrawalRequestService.GetAll(query);
        return Ok(response);
    }

    /// <summary>
    ///     Accept/Deny employee's withdrawal request
    /// </summary>
    /// <remarks>
    ///     Authorize: admin
    /// </remarks>
    [HttpPatch("{withdrawalRequestId}")]
    [Authorize(Roles = Roles.Admin)]
    public IActionResult Process(
        [FromRoute] Guid withdrawalRequestId,
        [FromBody] ProcessWithdrawalRequestRequest request)
    {
        _withdrawalRequestService.Process(withdrawalRequestId, request);
        return NoContent();
    }
}