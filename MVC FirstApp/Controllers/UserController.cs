using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FirstApp.Models.Services;
using MVC_FirstApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly AccountService _as;
        private readonly UserService _us;

        public UserController(AccountService accountService, UserService userEditService)
        {
            _as = accountService;
            _us = userEditService;
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var vm = _us.GetToEdit(id);

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(EditUserViewModel data)
        {
            //VM requirements
            if (!ModelState.IsValid)
            {
                return View(data);
            }

            var result = _us.Update(data);
            //Identity requirements
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Group");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(data);
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            var result = _as.DeleteUser(id);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Group");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction("Index", "Group");
        }
    }
}
