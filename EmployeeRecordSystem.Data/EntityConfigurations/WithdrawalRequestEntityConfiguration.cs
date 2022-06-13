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
    public class WithdrawalRequestEntityConfiguration : IEntityTypeConfiguration<WithdrawalRequest>
    {
        public void Configure(EntityTypeBuilder<WithdrawalRequest> builder)
        {
            builder.ToTable("withdrawal-requests");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(p => p.ProcessedAt).IsRequired(false);
            builder.Property(p => p.CreatedAt).IsRequired();
            builder.Property(p => p.WithdrawalRequestStatusTypeCode).IsRequired();

            builder.Property(p => p.Amount)
                .HasPrecision(18, 2);
        }
    }
}
