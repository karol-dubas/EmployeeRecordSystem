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
    [Authorize]
    public class UserController : Controller
    {
        private AccountService _as;
        private UserEditService _us;

        public UserController(AccountService accountService, UserEditService userEditService)
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
