using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EmployeeRecordSystem.Data.Configurations;
using EmployeeRecordSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Data
{
    public class MvcDbContext : IdentityDbContext<User>
    {
        public MvcDbContext(DbContextOptions<MvcDbContext> options) : base(options) { }

        public DbSet<UserBilling> UserBillings { get; set; }
        public DbSet<UserOperation> UserOperations { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Position> Positions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new GroupConfiguration());
            builder.ApplyConfiguration(new PositionConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
