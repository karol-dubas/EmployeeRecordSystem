using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVC_FirstApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace MVC_FirstApp.Data
{
    public class MvcDbContext : IdentityDbContext<User>
    {
        public MvcDbContext(DbContextOptions<MvcDbContext> options) : base(options) { }

        public DbSet<Billing> Billings { get; set; }
        public DbSet<UserOperation> UserOperations { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Position> Positions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .ToTable("Users") // rename table
                .Ignore(c => c.TwoFactorEnabled)
                .Ignore(c => c.SecurityStamp)
                .Ignore(c => c.PhoneNumberConfirmed)
                .Ignore(c => c.PhoneNumber)
                .Ignore(c => c.NormalizedEmail)
                .Ignore(c => c.LockoutEnd)
                .Ignore(c => c.LockoutEnabled)
                .Ignore(c => c.EmailConfirmed)
                .Ignore(c => c.Email)
                .Ignore(c => c.ConcurrencyStamp)
                .Ignore(c => c.AccessFailedCount);

            builder.Entity<User>().Property(c => c.FirstName).IsRequired();
            builder.Entity<User>().Property(c => c.LastName).IsRequired();
            builder.Entity<User>().Property(c => c.UserName).IsRequired();
            builder.Entity<User>().Property(c => c.PasswordHash).IsRequired();

            builder.Entity<Position>().Property(c => c.Name).IsRequired();

            builder.Entity<Group>().Property(c => c.Name).IsRequired();

            builder.Entity<IdentityRole>()
                .ToTable("Roles")
                .Ignore(c => c.ConcurrencyStamp);

            builder.Entity<IdentityUserRole<string>>()
                .ToTable("UserRoles");

            builder.Ignore<IdentityUserToken<string>>();
            builder.Ignore<IdentityUserLogin<string>>();
            builder.Ignore<IdentityUserClaim<string>>();
            builder.Ignore<IdentityRoleClaim<string>>();
        }
    }
}
