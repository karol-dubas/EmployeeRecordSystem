using AutoMapper;
using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmployeeRecordSystem.Server.Installers.ServiceAttributes;

namespace EmployeeRecordSystem.Server.Services
{
    public interface IRoleService
    {
        List<RoleDto> GetAll();
        void ChangeEmployeeRole(Guid employeeId, Guid roleId);
    }


    [ScopedRegistration]
    public class RoleService : BaseService, IRoleService
    {
        private readonly UserManager<Employee> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleService(ApplicationDbContext dbContext,
            IMapper mapper,
            UserManager<Employee> userManager,
            RoleManager<ApplicationRole> roleManager)
            : base(dbContext, mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<RoleDto> GetAll()
        {
            var roles = _roleManager.Roles.ToList();
            return _mapper.Map<List<RoleDto>>(roles);
        }

        public void ChangeEmployeeRole(Guid employeeId, Guid roleId)
        {
            var employee = _dbContext.Users.SingleOrDefault(e => e.Id == employeeId);
            // TODO: employee null check

            var newRole = _roleManager.FindByIdAsync(roleId.ToString()).Result;
            // TODO: role null check

            var currentRole = _userManager.GetRolesAsync(employee).Result.Single();
            _userManager.RemoveFromRoleAsync(employee, currentRole).Wait();

            _userManager.AddToRoleAsync(employee, newRole.Name).Wait();
        }
    }
}
