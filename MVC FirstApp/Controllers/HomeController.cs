using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC_FirstApp.Models;
using MVC_FirstApp.Models.Services;
using MVC_FirstApp.Models.ViewModels;

namespace MVC_FirstApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserService _us;
        private readonly UserManager<ApplicationUser> _um;
        public readonly AccountService _as;

        public HomeController(UserService userService,
            UserManager<ApplicationUser> userManager,
            AccountService accountService)
        {
            _us = userService;
            _um = userManager;
            _as = accountService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = _um.GetUserId(User);
            var vm = _us.GetUserDetails(userId);

            return View(vm);
        }

        [HttpGet]
        public IActionResult Password()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Password(PasswordViewModel data)
        {
            if (!data.PasswordConfirmed())
            {
                ModelState.AddModelError("", "Hasła muszą być takie same");
                return View();
            }

            var result = _as.ChangePassword(data.Id, data.CurrentPassword, data.NewPassword);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(data);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
