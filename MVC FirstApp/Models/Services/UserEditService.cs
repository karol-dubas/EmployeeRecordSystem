using MVC_FirstApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Models.Services
{
    public class UserEditService
    {
        private MvcDbContext _db;

        public UserEditService(MvcDbContext dbContext)
        {
            _db = dbContext;
        }

        public HomeUserViewModel GetUserDetails(string userId)
        {
            var user = _db.Users.Find(userId);

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
    }
}
