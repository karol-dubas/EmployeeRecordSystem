using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Server.Queries;
using EmployeeRecordSystem.Server.Services;
using EmployeeRecordSystem.Shared.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeRecordSystem.Server.Controllers
{
    [Route("api/withdrawal-requests")]
    [ApiController]
    public class WithdrawalRequestController : ControllerBase
    {
        private readonly IWithdrawalRequestService _withdrawalRequestService;

        public WithdrawalRequestController(IWithdrawalRequestService withdrawalRequestService)
        {
            _withdrawalRequestService = withdrawalRequestService;
        }

        /// <summary>
        /// Create
        /// </summary>
        [HttpPost("{employeeId}")]
        public IActionResult Create([FromRoute] Guid employeeId, [FromBody] CreateWithdrawalRequestRequest request)
        {
            var response = _withdrawalRequestService.Create(employeeId, request);
            return CreatedAtAction(nameof(GetAll), new { Id = response.Id }, response);
        }

        /// <summary>
        /// Get all
        /// </summary>
        [HttpGet]
        public IActionResult GetAll([FromQuery] WithdrawalRequestQuery query)
        {
            var response = _withdrawalRequestService.GetAll(query);
            return Ok(response);
        }

        /// <summary>
        /// Accept/Deny withdrawal request status
        /// </summary>
        [HttpPatch("{withdrawalRequestId}")]
        public IActionResult Process([FromRoute] Guid withdrawalRequestId, [FromBody] ProcessWithdrawalRequestRequest request)
        {
            _withdrawalRequestService.Process(withdrawalRequestId, request);
            return NoContent();
        }
    }
}
