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

        public UserController(AccountService accountService)
        {
            _as = accountService;
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(EditUserViewModel data)
        {
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
