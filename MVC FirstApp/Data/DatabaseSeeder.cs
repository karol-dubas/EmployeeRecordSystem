﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC_FirstApp.Data.Entities;
using MVC_FirstApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_FirstApp.Data
{
    public class DatabaseSeeder
    {
        private readonly MvcDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DatabaseSeeder(MvcDbContext dbContext,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Seed()
        {
            // Create database and migrate if can't connect 
            if (!_dbContext.Database.CanConnect())
            {
                RelationalDatabaseFacadeExtensions.Migrate(_dbContext.Database);
            }

            ApplyPendinMigrations();

            if (!_dbContext.Groups.Any())
            {
                var groups = GetGroups();
                _dbContext.Groups.AddRange(groups);
                _dbContext.SaveChanges();
            }

            if (!_dbContext.Positions.Any())
            {
                var positions = GetPositions();
                _dbContext.Positions.AddRange(positions);
                _dbContext.SaveChanges();
            }

            if (!_dbContext.Roles.Any())
            {
                _roleManager.CreateAsync(new(Roles.Admin)).Wait();
                _roleManager.CreateAsync(new(Roles.Worker)).Wait();
            }

            if (!_dbContext.Users.Any())
            {
                CreateDummyUsers();
                _dbContext.SaveChanges();
            }
        }

        private void CreateDummyUsers()
        {
            var groupA = _dbContext.Groups.Single(x => x.Id == 1);
            var groupB = _dbContext.Groups.Single(x => x.Id == 2);

            var workerPosition = _dbContext.Positions.Single(x => x.Id == 1);
            var groupLeaderPosition = _dbContext.Positions.Single(x => x.Id == 2);
            var chiefManagerPosition = _dbContext.Positions.Single(x => x.Id == 3);

            var user1 = new User()
            {
                FirstName = "Konrad",
                LastName = "Książka",
                UserName = "Konrad1",
            };
            _userManager.CreateAsync(user1, "1234").Wait();
            _userManager.AddToRoleAsync(user1, Roles.Admin).Wait();
            groupA.Users.Add(user1);
            chiefManagerPosition.Users.Add(user1);

            var user2 = new User()
            {
                FirstName = "Maciej",
                LastName = "Woźny",
                UserName = "Maciej1",
            };
            _userManager.CreateAsync(user2, "1234").Wait();
            _userManager.AddToRoleAsync(user2, Roles.Worker).Wait();
            groupB.Users.Add(user2);
            workerPosition.Users.Add(user2);

            var user3 = new User()
            {
                FirstName = "Leszek",
                LastName = "Maciejewski",
                UserName = "Leszek1",
            };
            _userManager.CreateAsync(user3, "1234").Wait();
            _userManager.AddToRoleAsync(user3, Roles.Worker).Wait();
            groupB.Users.Add(user3);
            workerPosition.Users.Add(user3);

            var user4 = new User()
            {
                FirstName = "Igor",
                LastName = "Szymański",
                UserName = "Igor1",
            };
            _userManager.CreateAsync(user4, "1234").Wait();
            _userManager.AddToRoleAsync(user4, Roles.Worker).Wait();
            groupB.Users.Add(user4);
            workerPosition.Users.Add(user4);

            var user5 = new User()
            {
                FirstName = "Karol",
                LastName = "Łysiak",
                UserName = "Karol1",
            };
            _userManager.CreateAsync(user5, "1234").Wait();
            _userManager.AddToRoleAsync(user5, Roles.Worker).Wait();
            groupB.Users.Add(user5);
            groupLeaderPosition.Users.Add(user5);
        }

        private IEnumerable<Position> GetPositions()
        {
            return new List<Position>()
            {
                new Position()
                {
                    Name = "Pracownik magazynu"
                },
                new Position()
                {
                    Name = "Kierownik grupy"
                },
                new Position()
                {
                    Name = "Kierownik główny"
                }
            };
        }

        private IEnumerable<Group> GetGroups()
        {
            return new List<Group>()
            {
                new Group()
                {
                    Name = "A"
                },
                new Group()
                {
                    Name = "B"
                },
                new Group()
                {
                    Name = "C"
                }
            };
        }

        private void ApplyPendinMigrations()
        {
            var pendingMigrations = _dbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                _dbContext.Database.Migrate();
            }
        }
    }
}