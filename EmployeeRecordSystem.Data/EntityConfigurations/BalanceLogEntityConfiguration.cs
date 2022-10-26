using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeRecordSystem.Data.Entities;

namespace EmployeeRecordSystem.Data.EntityConfigurations
{
    public class BalanceLogEntityConfiguration : IEntityTypeConfiguration<BalanceLog>
    {
        public void Configure(EntityTypeBuilder<BalanceLog> builder)
        {
            builder.ToTable("balance-logs");

            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(p => p.BalanceBefore)
                .HasPrecision(18, 2);

            builder.Property(p => p.BalanceAfter)
                .HasPrecision(18, 2);

            builder.Property(c => c.CreatedAt)
                .IsRequired();
        }
    }
}
