using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EmployeeRecordSystem.Data.Entities;

namespace EmployeeRecordSystem.Data.EntityConfigurations
{
    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("users");

            builder.Property(c => c.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.LastName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.BankAccountNumber)
                .HasMaxLength(34)
                .IsRequired(false);

            builder.Ignore(c => c.Role);

            builder.Ignore(c => c.FullName);

            builder.Property(c => c.UserName).IsRequired();
            builder.Property(c => c.PasswordHash).IsRequired();

            builder.HasOne(c => c.Group)
                .WithMany(c => c.Users)
                .HasForeignKey(c => c.GroupId);

            builder.HasOne(c => c.UserBilling)
                .WithOne(c => c.User)
                .HasForeignKey<UserBilling>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.BalanceLogs)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.WithdrawalRequests)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
