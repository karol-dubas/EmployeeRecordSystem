using AutoMapper;
using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Server.Exceptions;
using EmployeeRecordSystem.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmployeeRecordSystem.Server.Installers.Helpers.ServiceAttributes;

namespace EmployeeRecordSystem.Server.Services
{
    public interface IRoleService
    {
        List<RoleDto> GetAll();
        void ChangeEmployeeRole(Guid employeeId, Guid newRoleId);
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
            return Mapper.Map<List<RoleDto>>(roles);
        }

        public void ChangeEmployeeRole(Guid employeeId, Guid newRoleId)
        {
            var employee = DbContext.Users.SingleOrDefault(e => e.Id == employeeId);

            if (employee is null)
                throw new NotFoundException(nameof(employeeId), "Employee");

            var newRole = _roleManager.FindByIdAsync(newRoleId.ToString()).Result;

            if (newRole is null)
                throw new NotFoundException(nameof(newRoleId), "Role");

            string currentRole = _userManager.GetRolesAsync(employee).Result.Single();
            _userManager.RemoveFromRoleAsync(employee, currentRole).Wait();

            _userManager.AddToRoleAsync(employee, newRole.Name).Wait();
        }
    }
}
