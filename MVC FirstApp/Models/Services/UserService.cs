using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC_FirstApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Models.Services
{
    public class UserService
    {
        private readonly UserManager<ApplicationUser> _um;
        private readonly MvcDbContext _db;
        private readonly RoleManager<IdentityRole> _rm;

        public UserService(UserManager<ApplicationUser> userManager,
            MvcDbContext dbContext,
            RoleManager<IdentityRole> roleManager)
        {
            _um = userManager;
            _db = dbContext;
            _rm = roleManager;
        }

        public HomeUserViewModel GetUserDetails(string userId)
        {
            var user = _db.Users.Find(userId);

            if (user != null)
            {
                var vm = new HomeUserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Group = user.Group,
                    Position = user.Position
                };
                return vm;
            }

            return null;
        }


        public IdentityResult Update(EditUserViewModel data)
        {
            var user = _db.Users.Include(x => x.Billing).Where(x => x.Id == data.Id).Single();

            user.FirstName = data.FirstName;
            user.LastName = data.LastName;
            user.Group = data.Group;
            user.Position = data.Position;
            user.Billing.HourlyPay = data.HourlyPay;

            var result = _um.UpdateAsync(user).Result;

            return result;
        }
    }
}
