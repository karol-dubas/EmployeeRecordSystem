using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVC_FirstApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Data
{
    public class MvcDbContext : IdentityDbContext<ApplicationUser>
    {
        public MvcDbContext(DbContextOptions<MvcDbContext> options) : base(options) { }

        public DbSet<Billing> Billings { get; set; }
        public DbSet<AccountAction> AccountActions { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Position> Positions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
