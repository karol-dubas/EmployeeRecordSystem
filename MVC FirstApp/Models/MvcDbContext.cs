using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Models
{
    public class MvcDbContext : IdentityDbContext<ApplicationUser>
    {
        public MvcDbContext(DbContextOptions<MvcDbContext> options)
            : base(options) { }

        public DbSet<BillingEntity> Billings { get; set; }
        public DbSet<AccountHistoryEntity> Histories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
