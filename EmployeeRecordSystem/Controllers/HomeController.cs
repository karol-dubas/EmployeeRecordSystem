using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EmployeeRecordSystem.Constants;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Services;
using EmployeeRecordSystem.ViewModels;

namespace EmployeeRecordSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserDataService _userDataService;
        private readonly UserManager<User> _userManager;
        private readonly AccountService _accountService;

        public HomeController(UserDataService userDataService,
            UserManager<User> userManager,
            AccountService accountService)
        {
            _userDataService = userDataService;
            _userManager = userManager;
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var vm = _userDataService.GetUserDetails(userId);

            return View(vm);
        }

        [HttpGet]
        public IActionResult Password()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Password(PasswordViewModel data)
        {
            if (!data.PasswordConfirmed())
            {
                ModelState.AddModelError("", "Hasła muszą być takie same");
                return View();
            }

            var userId = _userManager.GetUserId(User);
            var result = await _accountService.ChangePassword(userId, data.CurrentPassword, data.NewPassword);

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

        [HttpGet]
        [Authorize(Roles = $"{Roles.Worker}, {Roles.Admin}")]
        public IActionResult Withdrawal()
        {
            var userId = _userManager.GetUserId(User);
            var vm = _userDataService.GetDataToWithdrawMoney(userId);

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = $"{Roles.Worker}, {Roles.Admin}")]
        public IActionResult Withdrawal(WithdrawalViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return View(data);
            }

            if (data.AmountToWithdraw <= 0)
            {
                ModelState.AddModelError("", "Wpisz kwotę powyżej 0");
                return View(data);
            }

            var userId = _userManager.GetUserId(User);
            var error = _userDataService.WithdrawMoney(data, userId);

            if (error)
            {
                ModelState.AddModelError("", "Nie możesz wypłacić więcej pieniędzy, niż masz na koncie");
                return View(data);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = $"{Roles.Worker}, {Roles.Admin}")]
        public IActionResult AccountHistory(string id)
        {
            var userId = _userManager.GetUserId(User);

            if (User.IsInRole(Roles.Admin) || userId == id)
            {
                var vm = _userDataService.GetUserHistory(id);

                return View(vm);
            }

            return Unauthorized();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
