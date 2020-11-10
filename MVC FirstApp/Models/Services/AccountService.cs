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

        public AccountService(UserManager<IdentityUser> userManager)
        {
            _um = userManager;
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
    }
}
