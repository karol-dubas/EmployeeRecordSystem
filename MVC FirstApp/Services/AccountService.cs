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
    public class AccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly MvcDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            MvcDbContext dbContext,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateUser(RegisterViewModel data)
        {
            var entity = new User()
            {
                UserName = data.UserName,
                FirstName = data.FirstName,
                LastName = data.LastName
            };

            var result = await _userManager.CreateAsync(entity, data.Password);

            return result;
        }

        //public async Task<SignInResult> SignIn(LoginViewModel data)
        //{
        //    var result = await _signInManager.PasswordSignInAsync(data.UserName, data.Password, false, false);

        //    return result;
        //}

        //public async void SignOut()
        //{
        //    await _signInManager.SignOutAsync();
        //}

        //public async Task<IdentityResult> DeleteUser(string id)
        //{
        //    var user = await _dbContext.Users.Include(x => x.Billing).Include(x => x.AccountActions).SingleOrDefaultAsync(x => x.Id == id);
        //    var userBilling = await _dbContext.Billings.FindAsync(user.Billing.Id);

        //    foreach (var history in user.AccountActions)
        //    {
        //        var row = await _dbContext.AccountActions.FindAsync(history.Id);
        //        _dbContext.AccountActions.Remove(row);
        //    }

        //    _dbContext.Billings.Remove(userBilling);
        //    var result = await _userManager.DeleteAsync(user);

        //    return result;
        //}

        //public async Task<IdentityResult> ChangePassword(string id, string currentPassword, string newPassword)
        //{
        //    var user = await _userManager.FindByIdAsync(id);

        //    var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        //    return result;
        //}

        //public async Task<EditRoleUsersViewModel> GetUsersInRole(string roleName)
        //{
        //    var role = await _roleManager.FindByNameAsync(roleName);

        //    var vm = new EditRoleUsersViewModel
        //    {
        //        RoleName = role.Name
        //    };

        //    foreach (var user in await _userManager.Users.ToListAsync())
        //    {
        //        if (await _userManager.IsInRoleAsync(user, role.Name))
        //            vm.Users.Add(string.Format($"- {user.FirstName} {user.LastName}, {user.UserName}"));
        //    }

        //    return vm;
        //}

        //public async Task<List<UserRoleViewModel>> GetToEditUsersInRole(string roleName)
        //{
        //    var role = await _roleManager.FindByNameAsync(roleName);

        //    var vmList = new List<UserRoleViewModel>();

        //    foreach (var user in await _userManager.Users.ToListAsync())
        //    {
        //        var vm = new UserRoleViewModel
        //        {
        //            UserName = user.UserName,
        //            UserId = user.Id,
        //            FullName = string.Format($"{user.FirstName} {user.LastName}")
        //        };

        //        if (await _userManager.IsInRoleAsync(user, role.Name))
        //            vm.IsSelected = true;
        //        else
        //        {
        //            vm.IsSelected = false;
        //        }

        //        vmList.Add(vm);
        //    }

        //    return vmList;
        //}

        //public async Task<IdentityResult> EditUsersInRole(List<UserRoleViewModel> model, string roleName)
        //{
        //    var role = await _roleManager.FindByNameAsync(roleName);
        //    IdentityResult result = null;

        //    for (var i = 0; i < model.Count; i++)
        //    {
        //        var user = await _userManager.FindByIdAsync(model[i].UserId);

        //        if (model[i].IsSelected && !await _userManager.IsInRoleAsync(user, role.Name))
        //            result = await _userManager.AddToRoleAsync(user, role.Name);
        //        else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
        //        {
        //            result = await _userManager.RemoveFromRoleAsync(user, role.Name);
        //        }
        //        else
        //        {
        //            continue;
        //        }
        //    }

        //    return result;
        //}
    }
}
