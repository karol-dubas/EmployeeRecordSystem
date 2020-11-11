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
        private UserManager<IdentityUser> _um;
        private SignInManager<IdentityUser> _sim;

        public AccountService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _um = userManager;
            _sim = signInManager;
        }

        public IdentityResult CreateUser(RegisterViewModel data)
        {
            var entity = new IdentityUser()
            {
                UserName = data.UserName,
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
    }
}
