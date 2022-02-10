using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EmployeeRecordSystem.Constants;
using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Helpers;
using EmployeeRecordSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Services
{
    public class UserDataService
    {
        private readonly UserManager<User> _userManager;
        private readonly MvcDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserDataService(UserManager<User> userManager,
            MvcDbContext dbContext,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _roleManager = roleManager;
        }

        public HomeUserViewModel GetUserDetails(string userId)
        {
            var user = _dbContext.Users
                .Include(x => x.UserBilling)
                .Include(x => x.Group)
                .Include(x => x.Position)
                .SingleOrDefault(x => x.Id == userId);

            var vm = new HomeUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Group = user.Group?.Name,
                Position = user.Position?.Name,
                TimeWorked = new TimeSpan(0, (int)user.UserBilling.MinutesWorked, 0).ToHoursAndMins(),
                Balance = string.Format($"{user.UserBilling.Balance}zł")
            };

            return vm;
        }

        public async Task<IdentityResult> Update(EditUserViewModel data)
        {
            var user = await _dbContext.Users
                .Include(x => x.UserBilling)
                .Include(x => x.Position)
                .Include(x => x.Group)
                .SingleOrDefaultAsync(x => x.Id == data.Id);

            var selectedGroup = _dbContext.Groups
                .SingleOrDefault(x => x.Id == data.SelectedGroupId);

            var selectedPosition = _dbContext.Positions
                .SingleOrDefault(x => x.Id == data.SelectedPositionId);

            user.FirstName = data.FirstName;
            user.LastName = data.LastName;
            user.Group = selectedGroup;
            user.Position = selectedPosition;
            user.UserBilling.HourlyPay = data.HourlyPay;

            var result = await _userManager.UpdateAsync(user);

            return result;
        }

        public WithdrawalViewModel GetDataToWithdrawMoney(string id)
        {
            var user = _dbContext.Users
                .Include(x => x.UserBilling)
                .SingleOrDefault(x => x.Id == id);

            var vm = new WithdrawalViewModel
            {
                CurrentBalance = string.Format($"{user.UserBilling.Balance} zł"),
                AmountToWithdraw = user.UserBilling.Balance
            };

            return vm;
        }

        public bool WithdrawMoney(WithdrawalViewModel data, string userId)
        {
            var user = _dbContext.Users
                .Include(x => x.UserBilling)
                .Include(x => x.UserOperations)
                .SingleOrDefault(x => x.Id == userId);

            var amount = data.AmountToWithdraw;
            var balanceAfter = user.UserBilling.Balance -= amount;

            if (balanceAfter < 0)
                return true;

            user.UserOperations.Add(new UserOperation
            {
                OperationType = AccountOperation.Withdrawal.GetDescription(),
                Amount = amount,
                BalanceAfter = balanceAfter,
                CreatedAt = DateTimeOffset.Now
            });

            _dbContext.SaveChanges();
            return false;
        }

        public List<AccountHistoryViewModel> GetUserHistory(string id)
        {
            var vm = _dbContext.Users
                .Include(x => x.UserOperations)
                .SingleOrDefault(x => x.Id == id)
                .UserOperations.Select(x => new AccountHistoryViewModel
                {
                    OperationType = x.OperationType,
                    Amount = string.Format($"{Math.Round(x.Amount, 2)} zł"),
                    BalanceAfter = string.Format($"{Math.Round(x.BalanceAfter, 2)} zł"),
                    CreatedAt = x.CreatedAt
                }).OrderByDescending(x => x.CreatedAt).ToList();

            return vm;
        }

        public async Task<EditUserViewModel> GetToEdit(string id)
        {
            var user = await _dbContext.Users
                .Include(x => x.UserBilling)
                .Include(x => x.Group)
                .Include(x => x.Position)
                .FirstOrDefaultAsync(x => x.Id == id);

            var userRolesList = new List<string>();

            foreach (var role in _roleManager.Roles.ToList())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                    userRolesList.Add(role.Name);
            }
            var userRoles = string.Join(", ", userRolesList);

            var groups = _dbContext.Groups
                .Select(x => new GroupViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsCurrent = user.GroupId == x.Id,
                }).ToList();

            var positions = _dbContext.Positions
                .Select(x => new PositionViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsCurrent = user.PositionId == x.Id,
                }).ToList();

            var vm = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Groups = groups,
                Positions = positions,
                Roles = userRoles,
                Balance = user.UserBilling.Balance,
                HourlyPay = user.UserBilling.HourlyPay,
                HoursMinutesWorked = new TimeSpan(0, (int)user.UserBilling.MinutesWorked, 0).ToHoursAndMins(),
            };

            return vm;
        }
    }
}

