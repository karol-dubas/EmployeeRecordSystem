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
using MVC_FirstApp.Models;
using MVC_FirstApp.Models.Services;
using MVC_FirstApp.Models.ViewModels;

namespace MVC_FirstApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserDataService userDataService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AccountService accountService;
        private readonly MvcDbContext dbContext;

        public HomeController(UserDataService userService,
            UserManager<ApplicationUser> userManager,
            AccountService accountService,
            MvcDbContext dbContext)
        {
            this.userDataService = userService;
            this.userManager = userManager;
            this.accountService = accountService;
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var userId = userManager.GetUserId(User);
            var vm = userDataService.GetUserDetails(userId);

            return View(vm);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Password()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Password(PasswordViewModel data)
        {
            if (!data.PasswordConfirmed())
            {
                ModelState.AddModelError("", "Hasła muszą być takie same");
                return View();
            }

            var result = await accountService.ChangePassword(data.Id, data.CurrentPassword, data.NewPassword);

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

        [HttpGet]
        [Authorize(Roles = "Pracownik, Administrator")]
        public IActionResult Withdrawal(string id)
        {
            var vm = userDataService.GetDataToWithdrawMoney(id);

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Pracownik, Administrator")]
        public IActionResult Withdrawal(WithdrawalViewModel data, string id)
        {
            if (!ModelState.IsValid)
            {
                return View(data);
            }

            if (data.AmountToWithdraw == 0)
            {
                ModelState.AddModelError("", "Wpisz kwotę powyżej 0");
                return View(data);
            }

            var user = dbContext.Users.Include(x => x.Billing).SingleOrDefault(x => x.Id == id);
            if (data.AmountToWithdraw > user.Billing.Balance)
            {
                ModelState.AddModelError("", "Nie możesz wypłacić więcej pieniędzy, niż masz na koncie");
                return View(data);
            }

            userDataService.WithdrawMoney(data.AmountToWithdraw, user);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Pracownik, Administrator")]
        public IActionResult AccountHistory(string id)
        {
            var vm = userDataService.GetUserHistory(id);

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
