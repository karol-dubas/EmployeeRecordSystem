using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC_FirstApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Models.Services
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
                var groupEnum = (GroupEnum)Enum.Parse(typeof(GroupEnum), group);

                vm = new UserHoursViewModel
                {
                    Users = dbContext.Users.Where(x => groupEnum == x.Group)
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
            var usersList = data.Users;

            if (data.AddHours == true)
            {
                foreach (var user in usersList)
                {
                    if (user.IsSelected)
                    {
                        var userFound = dbContext.Users.Include(x => x.Billing).SingleOrDefault(x => x.Id == user.UserId);

                        userFound.Billing.MinutesWorked += (data.MinutesToEdit + (data.HoursToEdit * 60));
                    }
                }
            }
            else
            {
                foreach (var user in usersList)
                {
                    if (user.IsSelected)
                    {
                        var userFound = dbContext.Users.Include(x => x.Billing).SingleOrDefault(x => x.Id == user.UserId);

                        var newTime = userFound.Billing.MinutesWorked -= (data.MinutesToEdit + (data.HoursToEdit * 60));

                        if (newTime < 0)
                        {
                            return true;
                        }
                    }
                }
            }

            dbContext.SaveChanges();
            return false;
        }
    }
}
