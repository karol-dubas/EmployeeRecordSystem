using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.ViewModels;

namespace EmployeeRecordSystem.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<User> _signInManager;

        public LoginController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel data)
        {
            //VM requirements
            if (!ModelState.IsValid)
            {
                return View(data);
            }

            var result = await _signInManager.PasswordSignInAsync(data.UserName, data.Password, false, false);

            //Identity requirements
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Nieprawidłowe dane");
                return View(data);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(LoginController.Index), "Login");
        }
    }
}
