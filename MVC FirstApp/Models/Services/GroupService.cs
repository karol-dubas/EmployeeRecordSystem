using Microsoft.AspNetCore.Identity;
using MVC_FirstApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Models.Services
{
    public class GroupService
    {
        private readonly MvcDbContext _db;

        public GroupService(MvcDbContext dbContext)
        {
            _db = dbContext;
        }

        public GroupListViewModel GetAll()
        {
            var vm = new GroupListViewModel
            {
                Accounts = _db.Users.Select(x => new GroupItemListViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Group = x.Group,
                    Position = x.Position
                }).ToList()
            };

            return vm;
        }
    }
}
