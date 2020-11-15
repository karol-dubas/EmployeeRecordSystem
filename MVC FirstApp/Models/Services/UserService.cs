using Microsoft.AspNetCore.Identity;
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

        public UserService(UserManager<ApplicationUser> userManager, MvcDbContext dbContext)
        {
            _um = userManager;
            _db = dbContext;
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

        public EditUserViewModel GetToEdit(string userId)
        {
            var user = _db.Users.Find(userId);

            var vm = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Group = user.Group,
                Position = user.Position
            };

            return vm;
        }

        public IdentityResult Update(EditUserViewModel data)
        {
            var user = _db.Users.Find(data.Id);

            user.FirstName = data.FirstName;
            user.LastName = data.LastName;
            user.Group = data.Group;
            user.Position = data.Position;

            var result = _um.UpdateAsync(user).Result;

            return result;
        }
    }
}
