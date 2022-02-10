using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EmployeeRecordSystem.Constants;
using EmployeeRecordSystem.Services;
using EmployeeRecordSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdministrationController : Controller
    {
        private readonly AccountService _accountService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserDataService _userDataService;

        public AdministrationController(AccountService accountService,
            RoleManager<IdentityRole> roleManager,
            UserDataService userDataService)
        {
            _accountService = accountService;
            _roleManager = roleManager;
            _userDataService = userDataService;
        }

        [HttpGet]
        public IActionResult RolesList()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> RolePreview(string name)
        {
            var vm = await _accountService.GetUsersInRole(name);
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string name)
        {
            ViewBag.roleName = name;

            var vm = await _accountService.GetToEditUsersInRole(name);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string name)
        {
            var result = await _accountService.EditUsersInRole(model, name);

            if (result.Succeeded || result is null)
            {
                return RedirectToAction("RolePreview", new { Name = name });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var vm = await _userDataService.GetToEdit(id);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel data)
        {
            //VM requirements
            if (!ModelState.IsValid)
            {
                return View(data);
            }

            var result = await _userDataService.Update(data);

            //Identity requirements
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(GroupController.Index), "Group");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _accountService.DeleteUser(id);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(GroupController.Index), "Group");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction(nameof(GroupController.Index), "Group");
        }
    }
}