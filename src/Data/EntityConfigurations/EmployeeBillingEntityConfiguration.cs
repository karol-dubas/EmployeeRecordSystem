using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using EmployeeRecordSystem.Data.Entities;

namespace EmployeeRecordSystem.Data.EntityConfigurations;

public class EmployeeBillingEntityConfiguration
    : IEntityTypeConfiguration<EmployeeBilling>
{
    public void Configure(EntityTypeBuilder<EmployeeBilling> builder)
    {
        builder.ToTable("employee-billings");

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(p => p.Balance)
            .HasPrecision(18, 2);

        builder.Property(p => p.HourlyPay)
            .HasPrecision(18, 2);

        // TimeSpan to ticks conversion
        // https://docs.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations#built-in-converters
        builder.Property(c => c.TimeWorked).HasConversion<long>();
    }
}