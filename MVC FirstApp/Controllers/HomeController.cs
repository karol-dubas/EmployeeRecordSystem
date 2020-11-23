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
        private readonly UserService _us;
        private readonly UserManager<ApplicationUser> _um;
        public readonly AccountService _as;
        public readonly MvcDbContext _db;

        public HomeController(UserService userService,
            UserManager<ApplicationUser> userManager,
            AccountService accountService,
            MvcDbContext dbContext)
        {
            _us = userService;
            _um = userManager;
            _as = accountService;
            _db = dbContext;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var userId = _um.GetUserId(User);
            var vm = _us.GetUserDetails(userId);

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

        [HttpGet]
        [Authorize(Roles = "Pracownik, Administrator")]
        public IActionResult Withdrawal(string id)
        {
            var user = _db.Users.Include(x => x.Billing).SingleOrDefault(x => x.Id == id);

            var vm = new WithdrawalViewModel
            {
                CurrentBalance = string.Format($"{user.Billing.Balance} zł"),
                AmountToWithdraw = user.Billing.Balance
            };

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

            var user = _db.Users.Include(x => x.Billing).SingleOrDefault(x => x.Id == id);

            if (data.AmountToWithdraw > user.Billing.Balance)
            {
                ModelState.AddModelError("", "Nie możesz wypłacić więcej pieniędzy, niż masz na koncie");
                return View(data);
            }
            else if (data.AmountToWithdraw == 0)
            {
                ModelState.AddModelError("", "Wpisz kwotę powyżej 0");
                return View(data);
            }
            else
            {
                var amount = data.AmountToWithdraw;
                var balanceAfter = user.Billing.Balance -= amount;

                user.AccountHistory = new List<AccountHistoryEntity>
                {
                    new AccountHistoryEntity
                    {
                        ActionType = "withdrawal",
                        Amount = (double)amount,
                        BalanceAfter = (double)balanceAfter,
                        Date = DateTime.Now
                    }
                };

                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Pracownik, Administrator")]
        public IActionResult AccountHistory(string id)
        {
            var vm = _db.Users.Include(x => x.AccountHistory).SingleOrDefault(x => x.Id == id)
                .AccountHistory.Select(x => new AccountHistoryViewModel
                {
                    ActionType = x.ActionType,
                    Amount = string.Format($"{Math.Round(x.Amount, 2)} zł"),
                    BalanceAfter = string.Format($"{Math.Round(x.BalanceAfter, 2)} zł"),
                    Date = x.Date
                }).OrderByDescending(x => x.Date).ToList();

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
