﻿using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Server.Queries;
using EmployeeRecordSystem.Server.Services;
using EmployeeRecordSystem.Shared.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace EmployeeRecordSystem.Server.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Get details with user billing
        /// </summary>
        [HttpGet("{employeeId}")]
        public IActionResult GetDetails([FromRoute] Guid employeeId)
        {
            var response = _employeeService.GetDetails(employeeId);
            return Ok(response);
        }

        /// <summary>
        /// Get all
        /// </summary>
        [HttpGet]
        public IActionResult GetAll([FromQuery] EmployeeQuery query)
        {
            var response = _employeeService.GetAll(query);
            return Ok(response);
        }

        /// <summary>
        /// Edit employee: FirstName, LastName, BankAccountNumber
        /// </summary>
        [HttpPut("{employeeId}")]             
        public IActionResult Edit([FromRoute] Guid employeeId, [FromBody] EditEmployeeRequest request)
        {
            _employeeService.Edit(employeeId, request);
            return NoContent();
        }

        /// <summary>
        /// Change employee hourly pay
        /// </summary>
        [HttpPatch("hourly-pay/{employeeId}")]
        public IActionResult ChangeHourlyPay([FromRoute] Guid employeeId, [FromBody] ChangeEmployeeHourlyPayRequest request)
        {
            _employeeService.ChangeHourlyPay(employeeId, request);
            return NoContent();
        }

        /// <summary>
        /// Change the work time of employees
        /// </summary>
        [HttpPatch("work-time/")]
        public IActionResult ChangeWorkTime([FromBody] ChangeEmployeesWorkTimeRequest request)
        {
            _employeeService.ChangeWorkTimes(request);
            return NoContent();
        }
    }
}