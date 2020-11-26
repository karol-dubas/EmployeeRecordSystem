using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_FirstApp.Models;
using MVC_FirstApp.Models.Services;
using MVC_FirstApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministrationController : Controller
    {
        private readonly AccountService accountService;
        private readonly RoleManager<IdentityRole> roleManager;

        public AdministrationController(AccountService accountService,
            RoleManager<IdentityRole> roleManager)
        {
            this.accountService = accountService;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult RolesList()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> RolePreview(string name)
        {
            var vm = await accountService.GetUsersInRole(name);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string name)
        {
            ViewBag.roleName = name;

            var vm = await accountService.GetToEditUsersInRole(name);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string name)
        {
            var result = await accountService.EditUsersInRole(model, name);

            if (result == null)
            {
                return RedirectToAction("RolePreview", new { Name = name });
            }

            if (result.Succeeded)
            {
                return RedirectToAction("RolePreview", new { Name = name });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }
    }
}