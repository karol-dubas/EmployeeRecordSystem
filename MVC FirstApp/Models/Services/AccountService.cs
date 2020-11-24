using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC_FirstApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Models.Services
{
    public class AccountService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly MvcDbContext dbContext;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            MvcDbContext dbContext,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dbContext = dbContext;
            this.roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateUser(RegisterViewModel data)
        {
            var entity = new ApplicationUser()
            {
                UserName = data.UserName,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Group = GroupEnum.None,
                Position = PositionEnum.None,
                Billing = new BillingEntity()
                {
                    HourlyPay = 0,
                    MinutesWorked = 0,
                    Balance = 0
                }
            };

            var result = await userManager.CreateAsync(entity, data.Password);

            return result;
        }

        public async Task<SignInResult> SignIn(LoginViewModel data)
        {
            var result = await signInManager.PasswordSignInAsync(data.UserName, data.Password, false, false);

            return result;
        }

        public async void SignOut()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> DeleteUser(string id)
        {
            var user = await dbContext.Users.Include(x => x.Billing).Include(x => x.AccountHistory).SingleOrDefaultAsync(x => x.Id == id);
            var userBilling = await dbContext.Billings.FindAsync(user.Billing.Id);

            foreach (var history in user.AccountHistory)
            {
                var row = await dbContext.Histories.FindAsync(history.Id);
                dbContext.Histories.Remove(row);
            }

            dbContext.Billings.Remove(userBilling);
            var result = await userManager.DeleteAsync(user);

            return result;
        }

        public async Task<IdentityResult> ChangePassword(string id, string currentPassword, string newPassword)
        {
            var user = await userManager.FindByIdAsync(id);

            var result = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            return result;
        }

        public async Task<EditRoleUsersViewModel> GetUsersInRole(string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);

            var vm = new EditRoleUsersViewModel
            {
                RoleName = role.Name
            };

            foreach (var user in await userManager.Users.ToListAsync())
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    vm.Users.Add(string.Format($"- {user.FirstName} {user.LastName}, {user.UserName}"));
                }
            }

            return vm;
        }

        public async Task<List<UserRoleViewModel>> GetToEditUsersInRole(string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);

            var vmList = new List<UserRoleViewModel>();

            foreach (var user in await userManager.Users.ToListAsync())
            {
                var vm = new UserRoleViewModel
                {
                    UserName = user.UserName,
                    UserId = user.Id,
                    FullName = string.Format($"{user.FirstName} {user.LastName}")
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    vm.IsSelected = true;
                }
                else
                {
                    vm.IsSelected = false;
                }

                vmList.Add(vm);
            }

            return vmList;
        }

        public async Task<IdentityResult> EditUsersInRole(List<UserRoleViewModel> model, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            IdentityResult result = null;

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

            }

            return result;
        }
    }
}
