using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserTaskService.Infrastructure.EntityConfiguration
{
    public class RequestStatusTypeEntityTypeConfiguration : IEntityTypeConfiguration<RequestStatusType>
    {
        public void Configure(EntityTypeBuilder<RequestStatusType> builder)
        {
            builder.ToTable("request-status-types");

            builder.HasKey(st => st.Code);

            builder.Property(st => st.Code)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(st => st.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasData(Enumeration.GetAll<RequestStatusType>().ToArray());
        }
    }
}
