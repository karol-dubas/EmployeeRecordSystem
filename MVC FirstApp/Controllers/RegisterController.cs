using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC_FirstApp.Models.Services;
using MVC_FirstApp.Models.ViewModels;

namespace MVC_FirstApp.Controllers
{
    public class RegisterController : Controller
    {
        public AccountService _as;

        public RegisterController(AccountService accountService)
        {
            _as = accountService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(RegisterViewModel data)
        {
            //VM requirements
            if (!ModelState.IsValid)
            {
                return View(data);
            }

            if (!data.PasswordConfirmed())
            {
                ModelState.AddModelError("", "Hasła muszą być takie same");
                return View(data);
            }

            var result = _as.CreateUser(data);
            //Identity requirements
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
    }
}
