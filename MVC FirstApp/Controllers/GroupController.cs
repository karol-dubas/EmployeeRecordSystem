using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_FirstApp.Models;
using MVC_FirstApp.Models.Services;
using Microsoft.EntityFrameworkCore;
using MVC_FirstApp.Models.ViewModels;

namespace MVC_FirstApp.Controllers
{
    public class GroupController : Controller
    {
        private readonly GroupService _gs;
        private readonly AccountService _as;
        private readonly UserService _us;
        private readonly UserManager<ApplicationUser> _um;
        private readonly RoleManager<IdentityRole> _rm;
        private readonly MvcDbContext _db;

        public GroupController(AccountService accountService,
            UserService userEditService,
            GroupService groupService,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            MvcDbContext dbContext)
        {
            _as = accountService;
            _us = userEditService;
            _gs = groupService;
            _um = userManager;
            _rm = roleManager;
            _db = dbContext;
        }

        [HttpGet]
        [Authorize(Roles = "Pracownik, Administrator")]
        public IActionResult Index()
        {
            var vm = _gs.GetAll();

            return View(vm);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult EditHours(string group, string id)
        {
            var vm = _gs.GetUsersInGroup(group, id);

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult EditHours(UserHoursViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return View(data);
            }

            var error = _gs.UpdateTime(data);

            if (error)
            {
                ModelState.AddModelError("", "Jeden z użytkowników miałby wartość przepracowanych godzin mniejszą od 0");

                return View(data);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditUser(string id)
        {
            //var vm = _us.GetToEdit(id);

            var user = await _db.Users.Include(x => x.Billing).FirstOrDefaultAsync(x => x.Id == id);

            var userRolesList = new List<string>();

            foreach (var role in _rm.Roles.ToList())
            {
                if (await _um.IsInRoleAsync(user, role.Name))
                {
                    userRolesList.Add(role.Name);
                }
            }
            var userRoles = string.Join(", ", userRolesList);

            var vm = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Group = user.Group,
                Position = user.Position,
                Roles = userRoles,
                Balance = user.Billing.Balance,
                HourlyPay = user.Billing.HourlyPay,
                HoursMinutesWorked = MinsToHoursMins(user.Billing.MinutesWorked)
            };

            return View(vm);
        }

        private static string MinsToHoursMins(long minutesToConvert)
        {
            var hours = minutesToConvert / 60;
            var minutes = minutesToConvert % 60;

            return $"{hours}h {minutes}min";
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult EditUser(EditUserViewModel data)
        {
            //VM requirements
            if (!ModelState.IsValid)
            {
                return View(data);
            }

            var result = _us.Update(data);
            //Identity requirements
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Group");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(data);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteUser(string id)
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

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddMoney()
        {
            foreach (var user in _db.Users.Include(x => x.Billing))
            {
                var amount = (decimal)user.Billing.HourlyPay / 60 * user.Billing.MinutesWorked;
                var balanceAfter = user.Billing.Balance += amount;
                user.Billing.MinutesWorked = 0;

                user.AccountHistory = new List<AccountHistoryEntity>
                {
                    new AccountHistoryEntity
                    {
                        ActionType = "renumeration",
                        Amount = (double)amount,
                        BalanceAfter = (double)balanceAfter,
                        Date = DateTime.Now
                    }
                };
            }

            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
