using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EmployeeRecordSystem.Data.Entities;

namespace EmployeeRecordSystem.Data.EntityConfigurations;

public class EmployeeEntityConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(c => c.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.BankAccountNumber)
            .HasMaxLength(34)
            .IsRequired(false);

        builder.Property(c => c.Note)
            .HasMaxLength(300)
            .IsRequired(false);

        builder.Ignore(c => c.Role);

        builder.Ignore(c => c.FullName);

        builder.Property(c => c.UserName).IsRequired();
        builder.Property(c => c.PasswordHash).IsRequired();

        builder.HasOne(c => c.Group)
            .WithMany(c => c.Employees)
            .HasForeignKey(c => c.GroupId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(c => c.EmployeeBilling)
            .WithOne(c => c.Employee)
            .HasForeignKey<EmployeeBilling>(c => c.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.BalanceLogs)
            .WithOne(c => c.Employee)
            .HasForeignKey(c => c.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.WithdrawalRequests)
            .WithOne(c => c.Employee)
            .HasForeignKey(c => c.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}