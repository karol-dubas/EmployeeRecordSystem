using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC_FirstApp.Data;
using MVC_FirstApp.Data.Entities;
using MVC_FirstApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Services
{
    public class GroupService
    {
        private readonly MvcDbContext dbContext;

        public GroupService(MvcDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public GroupListViewModel GetAll()
        {
            var vm = new GroupListViewModel
            {
                Accounts = dbContext.Users.Include(x => x.Billing).Select(x => new GroupItemListViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Group = x.Group,
                    Position = x.Position,
                    HoursMinutesWorked = MinsToHoursMins(x.Billing.MinutesWorked),
                    HourlyPay = x.Billing.HourlyPay

                }).OrderByDescending(x => x.Position).ToList()
            };

            return vm;
        }

        private static string MinsToHoursMins(long minutesToConvert)
        {
            var hours = minutesToConvert / 60;
            var minutes = minutesToConvert % 60;

            return $"{hours}h {minutes}min";
        }

        public UserHoursViewModel GetUsersInGroup(string group, string userId)
        {
            UserHoursViewModel vm;

            //vm for all group
            if (group != null)
            {
                var Group = (Group)Enum.Parse(typeof(Group), group);

                vm = new UserHoursViewModel
                {
                    Users = dbContext.Users.Where(x => Group == x.Group)
                     .Select(x => new HoursEditUserListViewModel
                     {
                         UserId = x.Id,
                         IsSelected = true,
                         FullName = string.Format($"{x.FirstName} {x.LastName}, {x.UserName}")
                     }).ToList()
                };

                return vm;
            }

            //vm only for 1 user
            vm = new UserHoursViewModel
            {
                Users = dbContext.Users.Where(x => userId == x.Id)
                  .Select(x => new HoursEditUserListViewModel
                  {
                      UserId = x.Id,
                      IsSelected = true,
                      FullName = string.Format($"{x.FirstName} {x.LastName}, {x.UserName}")
                  }).ToList()
            };

            return vm;
        }

        public bool UpdateTime(UserHoursViewModel data)
        {
            var selectedUsers = data.Users.Where(x => x.IsSelected == true);

            if (data.AddHours == true)
            {
                foreach (var user in selectedUsers)
                {
                    var userFound = dbContext.Users.Include(x => x.Billing).SingleOrDefault(x => x.Id == user.UserId);

                    userFound.Billing.MinutesWorked += data.MinutesToEdit + data.HoursToEdit * 60;
                }
            }
            else
            {
                foreach (var user in selectedUsers)
                {
                    var userFound = dbContext.Users.Include(x => x.Billing).SingleOrDefault(x => x.Id == user.UserId);

                    var newTime = userFound.Billing.MinutesWorked -= data.MinutesToEdit + data.HoursToEdit * 60;

                    if (newTime < 0)
                        return true;
                }
            }

            dbContext.SaveChanges();
            return false;
        }

        public void AddMoney()
        {
            var users = dbContext.Users.Include(x => x.Billing).Include(x => x.AccountHistory)
                .Where(x => x.Billing.MinutesWorked > 0);

            foreach (var user in users)
            {
                var amount = user.Billing.HourlyPay / 60 * user.Billing.MinutesWorked;
                var balanceAfter = user.Billing.Balance += amount;
                user.Billing.MinutesWorked = 0;

                user.AccountHistory.Add(new AccountAction
                {
                    Action = Data.Enums.Action.Salary.ToString(),
                    Amount = amount,
                    BalanceAfter = balanceAfter,
                    CreatedAt = DateTime.Now
                });
            }

            dbContext.SaveChanges();
        }
    }
}
