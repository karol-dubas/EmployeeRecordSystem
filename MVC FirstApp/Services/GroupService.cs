using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC_FirstApp.Constants;
using MVC_FirstApp.Data;
using MVC_FirstApp.Data.Entities;
using MVC_FirstApp.Helpers;
using MVC_FirstApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Services
{
    public class GroupService
    {
        private readonly MvcDbContext _dbContext;

        public GroupService(MvcDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<GroupListViewModel> GetAll()
        {
            var vm = new List<GroupListViewModel>();
            vm.Add(GetUsersWithoutGroup());
            vm.AddRange(GetGroupsWithUsers());

            return vm;
        }

        private GroupListViewModel GetUsersWithoutGroup()
        {
            var usersWithoutGroup = _dbContext.Users
                .Where(u => u.GroupId == null)
                .OrderByDescending(u => u.PositionId)
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    HourlyPay = u.UserBilling.HourlyPay,
                    TimeWorked = new TimeSpan(0, (int)u.UserBilling.MinutesWorked, 0).ToHoursAndMins(),
                    LastName = u.LastName,
                    PositionName = u.Position.Name ?? "-"
                });

            return new GroupListViewModel()
            {
                Group = new GroupViewModel()
                {
                    Name = "Bez grupy"
                },
                UsersInGroup = usersWithoutGroup
            };
        }

        private IEnumerable<GroupListViewModel> GetGroupsWithUsers()
        {
            var groupsWithUsers = new List<GroupListViewModel>();

            var groups = _dbContext.Groups
                .Include(x => x.Users)
                    .ThenInclude(x => x.UserBilling)
                .Include(x => x.Users)
                    .ThenInclude(x => x.Position)
                .ToList();

            foreach (var group in groups)
            {
                var usersInGroupVm = group.Users
                    .OrderByDescending(u => u.PositionId)
                    .Select(u => new UserViewModel
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        HourlyPay = u.UserBilling.HourlyPay,
                        TimeWorked = new TimeSpan(0, (int)u.UserBilling.MinutesWorked, 0).ToHoursAndMins(),
                        LastName = u.LastName,
                        PositionName = u.Position?.Name ?? "-"
                    });

                var vmItem = new GroupListViewModel()
                {
                    Group = new GroupViewModel()
                    {
                        Name = group.Name,
                        Id = group.Id
                    },
                    UsersInGroup = usersInGroupVm
                };

                groupsWithUsers.Add(vmItem);
            }

            return groupsWithUsers;
        }

        public UserHoursViewModel GetGroupUsers(long groupId)
        {
            var group = _dbContext.Groups
                .Include(x => x.Users)
                .Single(x => x.Id == groupId);

            var groupUsers = group.Users
                .Select(x => new HoursEditUserListViewModel
                {
                    UserId = x.Id,
                    IsSelected = true,
                    FullName = string.Format($"{x.FirstName} {x.LastName}, {x.UserName}")
                }).ToList();

            var vm = new UserHoursViewModel
            {
                Users = groupUsers
            };

            return vm;
        }

        public bool UpdateTime(UserHoursViewModel data)
        {
            var selectedUserIds = data.Users
                .Where(x => x.IsSelected == true)
                .Select(u => u.UserId);

            var dbUsers = _dbContext.Users
                .Include(u => u.UserBilling)
                .Where(u => selectedUserIds.Contains(u.Id))
                .ToList();

            if (data.WorkTimeOperation == WorkTimeOperation.Add)
            {
                dbUsers.ForEach(u =>
                {
                    u.UserBilling.MinutesWorked += data.MinutesToEdit + data.HoursToEdit * 60;
                });
            }

            if (data.WorkTimeOperation == WorkTimeOperation.Substract)
            {
                foreach (var user in dbUsers)
                {
                    var newTime = user.UserBilling.MinutesWorked -= data.MinutesToEdit + data.HoursToEdit * 60;

                    if (newTime < 0)
                        return true;
                }
            }

            _dbContext.SaveChanges();
            return false;
        }

        public void AddMoney()
        {
            var users = _dbContext.Users
                .Include(x => x.UserBilling)
                .Include(x => x.UserOperations)
                .Where(x => x.UserBilling.MinutesWorked > 0);

            foreach (var user in users)
            {
                var amount = user.UserBilling.HourlyPay / 60 * user.UserBilling.MinutesWorked;
                var balanceAfter = user.UserBilling.Balance += amount;
                user.UserBilling.MinutesWorked = 0;

                user.UserOperations.Add(new UserOperation
                {
                    OperationType = AccountOperation.Salary.GetDescription(),
                    Amount = amount,
                    BalanceAfter = balanceAfter,
                    CreatedAt = DateTimeOffset.Now
                });
            }

            _dbContext.SaveChanges();
        }
    }
}
