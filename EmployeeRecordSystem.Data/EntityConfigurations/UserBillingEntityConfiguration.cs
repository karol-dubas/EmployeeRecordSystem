﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using EmployeeRecordSystem.Data.Entities;

namespace EmployeeRecordSystem.Data.EntityConfigurations
{
    public class UserBillingEntityConfiguration
        : IEntityTypeConfiguration<UserBilling>
    {
        public void Configure(EntityTypeBuilder<UserBilling> builder)
        {
            builder.ToTable("user-billings");

            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            // TimeSpan to ticks conversion
            // https://docs.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations#built-in-converters
            builder.Property(c => c.Balance).HasConversion<long>();
        }
    }
}