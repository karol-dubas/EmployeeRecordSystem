using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC_FirstApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Models.Services
{
    public class UserDataService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly MvcDbContext dbContext;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserDataService(UserManager<ApplicationUser> userManager,
            MvcDbContext dbContext,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.roleManager = roleManager;
        }

        public HomeUserViewModel GetUserDetails(string userId)
        {
            var user = dbContext.Users.Include(x => x.Billing).SingleOrDefault(x => x.Id == userId);

            var vm = new HomeUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Group = user.Group,
                Position = user.Position,
                HoursMinutesWorked = MinsToHoursMins(user.Billing.MinutesWorked),
                Balance = string.Format($"{user.Billing.Balance}zł")
            };

            return vm;
        }

        private static string MinsToHoursMins(long minutesToConvert)
        {
            var hours = minutesToConvert / 60;
            var minutes = minutesToConvert % 60;

            return $"{hours}h {minutes}min";
        }

        public async Task<IdentityResult> Update(EditUserViewModel data)
        {
            var user = await dbContext.Users.Include(x => x.Billing).SingleOrDefaultAsync(x => x.Id == data.Id);

            user.FirstName = data.FirstName;
            user.LastName = data.LastName;
            user.Group = data.Group;
            user.Position = data.Position;
            user.Billing.HourlyPay = data.HourlyPay;

            var result = await userManager.UpdateAsync(user);

            return result;
        }

        public WithdrawalViewModel GetDataToWithdrawMoney(string id)
        {
            var user = dbContext.Users.Include(x => x.Billing).SingleOrDefault(x => x.Id == id);

            var vm = new WithdrawalViewModel
            {
                CurrentBalance = string.Format($"{user.Billing.Balance} zł"),
                AmountToWithdraw = user.Billing.Balance
            };

            return vm;
        }

        public void WithdrawMoney(decimal amountToWithdraw, ApplicationUser user)
        {
            var amount = amountToWithdraw;
            var balanceAfter = user.Billing.Balance -= amount;

            user.AccountHistory = new List<AccountHistoryEntity>
            {
                 new AccountHistoryEntity
                 {
                    ActionType = "wypłata",
                    Amount = (double)amount,
                    BalanceAfter = (double)balanceAfter,
                    Date = DateTime.Now
                 }
            };

            dbContext.SaveChanges();
        }

        public List<AccountHistoryViewModel> GetUserHistory(string id)
        {
            var vm = dbContext.Users.Include(x => x.AccountHistory).SingleOrDefault(x => x.Id == id)
               .AccountHistory.Select(x => new AccountHistoryViewModel
               {
                   ActionType = x.ActionType,
                   Amount = string.Format($"{Math.Round(x.Amount, 2)} zł"),
                   BalanceAfter = string.Format($"{Math.Round(x.BalanceAfter, 2)} zł"),
                   Date = x.Date
               }).OrderByDescending(x => x.Date).ToList();

            return vm;
        }

        public void AddMoney()
        {
            var users = dbContext.Users.Include(x => x.Billing);

            foreach (var user in users)
            {
                var amount = (decimal)user.Billing.HourlyPay / 60 * user.Billing.MinutesWorked;
                var balanceAfter = user.Billing.Balance += amount;
                user.Billing.MinutesWorked = 0;

                user.AccountHistory = new List<AccountHistoryEntity>
                {
                    new AccountHistoryEntity
                    {
                        ActionType = "wynagrodzenie",
                        Amount = (double)amount,
                        BalanceAfter = (double)balanceAfter,
                        Date = DateTime.Now
                    }
                };
            }

            dbContext.SaveChanges();
        }

        public async Task<EditUserViewModel> GetToEdit(string id)
        {
            var user = await dbContext.Users.Include(x => x.Billing).FirstOrDefaultAsync(x => x.Id == id);

            var userRolesList = new List<string>();

            foreach (var role in roleManager.Roles.ToList())
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
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

            return vm;
        }
    }
}

