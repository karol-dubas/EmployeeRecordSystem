using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC_FirstApp.Services;
using MVC_FirstApp.ViewModels;

namespace MVC_FirstApp.Controllers
{
    public class LoginController : Controller
    {
        //        private readonly AccountService accountService;

        //        public LoginController(AccountService accountService)
        //        {
        //            this.accountService = accountService;
        //        }

        //        [HttpGet]
        //        public IActionResult Index()
        //        {
        //            return View();
        //        }

        //        [HttpPost]
        //        public async Task<IActionResult> Index(LoginViewModel data)
        //        {
        //            //VM requirements
        //            if (!ModelState.IsValid)
        //            {
        //                return View(data);
        //            }

        //            var result = await accountService.SignIn(data);

        //            //Identity requirements
        //            if (result.Succeeded)
        //            {
        //                return RedirectToAction("Index", "Home");
        //            }

        //            ModelState.AddModelError("", "Nieprawidłowe dane");

        //            return View(data);
        //        }

        //        [HttpGet]
        //        public IActionResult Logout()
        //        {
        //            accountService.SignOut();

        //            return RedirectToAction("Index", "Login");
        //        }
    }
}
