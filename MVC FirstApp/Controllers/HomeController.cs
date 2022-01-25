using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVC_FirstApp.Data.Entities;
using MVC_FirstApp.Services;
using MVC_FirstApp.ViewModels;

namespace MVC_FirstApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserDataService userDataService;
        private readonly UserManager<User> userManager;
        private readonly AccountService accountService;

        public HomeController(UserDataService userDataService,
            UserManager<User> userManager,
            AccountService accountService)
        {
            this.userDataService = userDataService;
            this.userManager = userManager;
            this.accountService = accountService;
        }

        //[HttpGet]
        //[Authorize]
        //public IActionResult Index()
        //{
        //    var userId = userManager.GetUserId(User);
        //    var vm = userDataService.GetUserDetails(userId);

        //    return View(vm);
        //}

        //[HttpGet]
        //[Authorize]
        //public IActionResult Password()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[Authorize]
        //public async Task<IActionResult> Password(PasswordViewModel data)
        //{
        //    if (!data.PasswordConfirmed())
        //    {
        //        ModelState.AddModelError("", "Hasła muszą być takie same");
        //        return View();
        //    }

        //    var result = await accountService.ChangePassword(data.Id, data.CurrentPassword, data.NewPassword);

        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError("", error.Description);
        //    }

        //    return View(data);
        //}

        //[HttpGet]
        //[Authorize(Roles = "Pracownik, Administrator")]
        //public IActionResult Withdrawal()
        //{
        //    var userId = userManager.GetUserId(User);
        //    var vm = userDataService.GetDataToWithdrawMoney(userId);

        //    return View(vm);
        //}

        //[HttpPost]
        //[Authorize(Roles = "Pracownik, Administrator")]
        //public IActionResult Withdrawal(WithdrawalViewModel data)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(data);
        //    }

        //    if (data.AmountToWithdraw == 0)
        //    {
        //        ModelState.AddModelError("", "Wpisz kwotę powyżej 0");
        //        return View(data);
        //    }

        //    var userId = userManager.GetUserId(User);
        //    var error = userDataService.WithdrawMoney(data, userId);

        //    if (error)
        //    {
        //        ModelState.AddModelError("", "Nie możesz wypłacić więcej pieniędzy, niż masz na koncie");
        //        return View(data);
        //    }

        //    return RedirectToAction("Index");
        //}

        //[HttpGet]
        //[Authorize(Roles = "Pracownik, Administrator")]
        //public IActionResult AccountHistory(string id)
        //{
        //    var userId = userManager.GetUserId(User);

        //    if (User.IsInRole("Administrator") || userId == id)
        //    {
        //        var vm = userDataService.GetUserHistory(id);

        //        return View(vm);
        //    }

        //    return Unauthorized();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
