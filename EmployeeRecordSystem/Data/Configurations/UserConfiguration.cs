using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EmployeeRecordSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .ToTable("Users") // rename table
                .Ignore(c => c.TwoFactorEnabled)
                .Ignore(c => c.PhoneNumberConfirmed)
                .Ignore(c => c.PhoneNumber)
                .Ignore(c => c.NormalizedEmail)
                .Ignore(c => c.LockoutEnd)
                .Ignore(c => c.LockoutEnabled)
                .Ignore(c => c.EmailConfirmed)
                .Ignore(c => c.Email)
                .Ignore(c => c.AccessFailedCount);

            builder.Property(c => c.FirstName).IsRequired();
            builder.Property(c => c.LastName).IsRequired();
            builder.Property(c => c.UserName).IsRequired();
            builder.Property(c => c.PasswordHash).IsRequired();
        }
    }
}
