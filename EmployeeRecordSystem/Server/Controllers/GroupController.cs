using AutoMapper;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Server.Services;
using EmployeeRecordSystem.Shared.Queries;
using EmployeeRecordSystem.Shared.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace EmployeeRecordSystem.Server.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        /// <summary>
        /// Create
        /// </summary>
        [HttpPost]
        public IActionResult Create([FromBody] CreateGroupRequest request)
        {
            var response = _groupService.Create(request);
            return CreatedAtAction(nameof(GetAll), new { Id = response.Id }, response);
        }

        /// <summary>
        /// Get all
        /// </summary>
        [HttpGet]
        public IActionResult GetAll([FromQuery] GroupQuery query)
        {
            var response = _groupService.GetAll(query);
            return Ok(response);
        }

        /// <summary>
        /// Rename
        /// </summary>
        [HttpPatch("{groupId}")]
        public IActionResult Rename([FromRoute] Guid groupId, [FromBody] RenameGroupRequest request)
        {
            var response = _groupService.Rename(groupId, request);
            return Ok(response);
        }

        /// <summary>
        /// Assign employee to a group
        /// </summary>
        [HttpPatch("{groupId}/employee/{employeeId}")]
        public IActionResult AssignEmployeeToGroup([FromRoute] Guid groupId, [FromRoute] Guid employeeId)
        {
            _groupService.AssignEmployeeToGroup(groupId, employeeId);
            return NoContent();
        }

        /// <summary>
        /// Remove employee from the group
        /// </summary>
        [HttpDelete("employee/{employeeId}")]
        public IActionResult RemoveEmployeeFromGroup([FromRoute] Guid employeeId)
        {
            _groupService.RemoveEmployeeFromGroup(employeeId);
            return NoContent();
        }

        /// <summary>
        /// Delete
        /// </summary>
        [HttpDelete("{groupId}")]
        public IActionResult Delete([FromRoute] Guid groupId)
        {
            _groupService.Delete(groupId);
            return NoContent();
        }
    }
}
