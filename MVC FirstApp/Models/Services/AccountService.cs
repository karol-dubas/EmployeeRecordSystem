using Microsoft.AspNetCore.Identity;
using MVC_FirstApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Models.Services
{
    public class AccountService
    {
        private UserManager<ApplicationUser> _um;
        private SignInManager<ApplicationUser> _sim;
        private MvcDbContext _db;

        public AccountService(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            MvcDbContext dbContext)
        {
            _um = userManager;
            _sim = signInManager;
            _db = dbContext;
        }

        public IdentityResult CreateUser(RegisterViewModel data)
        {
            var entity = new ApplicationUser()
            {
                UserName = data.UserName,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Group = GroupEnum.None,
                Position = PositionEnum.None
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
            var user = _db.Users.Find(id);

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
