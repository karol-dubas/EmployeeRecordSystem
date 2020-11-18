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
        private readonly UserManager<ApplicationUser> _um;
        private readonly SignInManager<ApplicationUser> _sim;
        private readonly MvcDbContext _db;
        private readonly RoleManager<IdentityRole> _rm;

        public AccountService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            MvcDbContext dbContext,
            RoleManager<IdentityRole> roleManager)
        {
            _um = userManager;
            _sim = signInManager;
            _db = dbContext;
            _rm = roleManager;
        }

        public IdentityResult CreateUser(RegisterViewModel data)
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

            var result = _um.CreateAsync(entity, data.Password).Result;

            return result;
        }

        public SignInResult SignIn(LoginViewModel data)
        {
            var result = _sim.PasswordSignInAsync(data.UserName, data.Password, false, false).Result;

            return result;
        }

        public void SignOut()
        {
            _sim.SignOutAsync();
        }

        public IdentityResult DeleteUser(string id)
        {
            var user = _db.Users.Include(x => x.Billing).Where(x => x.Id == id).Single();
            var userBilling = _db.Billings.Find(user.Billing.Id);

            _db.Billings.Remove(userBilling);
            var result = _um.DeleteAsync(user).Result;

            return result;
        }

        public IdentityResult ChangePassword(string id, string currentPassword, string newPassword)
        {
            var user = _db.Users.Find(id);

            var result = _um.ChangePasswordAsync(user, currentPassword, newPassword).Result;

            return result;
        }
    }
}
