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
        private readonly AccountService _as;
        private readonly RoleManager<IdentityRole> _rm;
        private readonly UserManager<ApplicationUser> _um;

        public AdministrationController(AccountService accountService,
            RoleManager<IdentityRole> RoleManager,
            UserManager<ApplicationUser> userManager)
        {
            _as = accountService;
            _rm = RoleManager;
            _um = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> CreateRoles()
        {
            if (!_rm.RoleExistsAsync("Administrator").Result)
            {
                await _rm.CreateAsync(new IdentityRole { Name = "Administrator" });
            }
            if (!_rm.RoleExistsAsync("Pracownik").Result)
            {
                await _rm.CreateAsync(new IdentityRole { Name = "Pracownik" });
            }

            return RedirectToAction("RolesList");
        }

        [HttpGet]
        public IActionResult RolesList()
        {
            var roles = _rm.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string name)
        {
            var role = await _rm.FindByNameAsync(name);

            var vm = new EditRoleUsersViewModel
            {
                RoleName = role.Name
            };

            foreach (var user in await _um.Users.ToListAsync())
            {
                if (await _um.IsInRoleAsync(user, role.Name))
                {
                    vm.Users.Add(string.Format($"- {user.FirstName} {user.LastName}, {user.UserName}"));
                }
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string name)
        {
            ViewBag.roleName = name;

            var role = await _rm.FindByNameAsync(name);

            var vmList = new List<UserRoleViewModel>();

            foreach (var user in await _um.Users.ToListAsync())
            {
                var vm = new UserRoleViewModel
                {
                    UserName = user.UserName,
                    UserId = user.Id,
                    FullName = string.Format($"{user.FirstName} {user.LastName}")
                };

                if(await _um.IsInRoleAsync(user, role.Name))
                {
                    vm.IsSelected = true;
                }
                else
                {
                    vm.IsSelected = false;
                }

                vmList.Add(vm);
            }

            return View(vmList);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string name)
        {
            var role = await _rm.FindByNameAsync(name);

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _um.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await _um.IsInRoleAsync(user, role.Name)))
                {
                    result = await _um.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await _um.IsInRoleAsync(user, role.Name))
                {
                    result = await _um.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditRole", new { Name = name });
                    }
                }
            }

            return RedirectToAction("EditRole", new { Name = name });
        }
    }
}