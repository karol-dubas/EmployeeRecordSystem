using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC_FirstApp.Services;
using MVC_FirstApp.ViewModels;

namespace MVC_FirstApp.Controllers
{
    public class RegisterController : Controller
    {
        private readonly AccountService _accountService;

        public RegisterController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return View(data);
            }

            if (!data.PasswordConfirmed())
            {
                ModelState.AddModelError("", "Hasła muszą być takie same");
                return View(data);
            }

            var result = await _accountService.CreateUser(data);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(data);
        }
    }
}
