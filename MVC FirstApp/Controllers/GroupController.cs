using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_FirstApp.ViewModels;
using MVC_FirstApp.Services;

namespace MVC_FirstApp.Controllers
{
    public class GroupController : Controller
    {
        //        private readonly AccountService accountService;
        //        private readonly UserDataService userDataService;
        //        private readonly GroupService groupService;

        //        public GroupController(AccountService accountService,
        //            UserDataService userDataService,
        //            GroupService groupService)
        //        {
        //            this.accountService = accountService;
        //            this.userDataService = userDataService;
        //            this.groupService = groupService;
        //        }

        //        [HttpGet]
        //        [Authorize(Roles = "Pracownik, Administrator")]
        //        public IActionResult Index()
        //        {
        //            var vm = groupService.GetAll();

        //            return View(vm);
        //        }

        //        [HttpGet]
        //        [Authorize(Roles = "Administrator")]
        //        public IActionResult EditHours(string group, string id)
        //        {
        //            var vm = groupService.GetUsersInGroup(group, id);

        //            return View(vm);
        //        }

        //        [HttpPost]
        //        [Authorize(Roles = "Administrator")]
        //        public IActionResult EditHours(UserHoursViewModel data)
        //        {
        //            if (!ModelState.IsValid)
        //            {
        //                return View(data);
        //            }

        //            var error = groupService.UpdateTime(data);

        //            if (error)
        //            {
        //                ModelState.AddModelError("", "Jeden z użytkowników miałby wartość przepracowanych godzin mniejszą od 0");

        //                return View(data);
        //            }

        //            return RedirectToAction("Index");
        //        }

        //        [HttpGet]
        //        [Authorize(Roles = "Administrator")]
        //        public async Task<IActionResult> EditUser(string id)
        //        {
        //            var vm = await userDataService.GetToEdit(id);

        //            return View(vm);
        //        }

        //        [HttpPost]
        //        [Authorize(Roles = "Administrator")]
        //        public async Task<IActionResult> EditUser(EditUserViewModel data)
        //        {
        //            //VM requirements
        //            if (!ModelState.IsValid)
        //            {
        //                return View(data);
        //            }

        //            var result = await userDataService.Update(data);

        //            //Identity requirements
        //            if (result.Succeeded)
        //            {
        //                return RedirectToAction("Index", "Group");
        //            }

        //            foreach (var error in result.Errors)
        //            {
        //                ModelState.AddModelError("", error.Description);
        //            }

        //            return View(data);
        //        }

        //        [HttpGet]
        //        [Authorize(Roles = "Administrator")]
        //        public async Task<IActionResult> DeleteUser(string id)
        //        {
        //            var result = await accountService.DeleteUser(id);

        //            if (result.Succeeded)
        //            {
        //                return RedirectToAction("Index", "Group");
        //            }

        //            foreach (var error in result.Errors)
        //            {
        //                ModelState.AddModelError("", error.Description);
        //            }

        //            return RedirectToAction("Index", "Group");
        //        }

        //        [HttpGet]
        //        [Authorize(Roles = "Administrator")]
        //        public IActionResult AddMoney()
        //        {
        //            groupService.AddMoney();

        //            return RedirectToAction("Index");
        //        }
    }
}
