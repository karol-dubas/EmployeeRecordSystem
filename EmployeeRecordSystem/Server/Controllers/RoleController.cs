using EmployeeRecordSystem.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeRecordSystem.Server.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Get all
        /// </summary>
        [HttpGet]
        public IActionResult GetAll()
        {
            var response = _roleService.GetAll();
            return Ok(response);
        }

        /// <summary>
        /// Change employee's role
        /// </summary>
        [HttpPatch("{roleId}/employee/{employeeId}")]
        public IActionResult ChangeEmployeeRole(Guid employeeId, Guid roleId)
        {
            _roleService.ChangeEmployeeRole(employeeId, roleId);
            return NoContent();
        }
    }
}
