using AutoMapper;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Server.Services;
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

        [HttpPost]
        public IActionResult Create([FromBody] CreateGroupRequest request)
        {
            var response = _groupService.Create(request);
            return CreatedAtAction(nameof(Get), new { groupId = response.Id} , response);
        }

        [HttpGet("{groupId}")]
        public IActionResult Get([FromRoute] Guid groupId)
        {
            var response = _groupService.Get(groupId);
            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = _groupService.GetAll();
            return Ok(response);
        }
    }
}
