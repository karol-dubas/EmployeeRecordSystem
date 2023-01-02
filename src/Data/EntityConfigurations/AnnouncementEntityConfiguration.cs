using EmployeeRecordSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeRecordSystem.Data.EntityConfigurations;

public class AnnouncementEntityConfiguration : IEntityTypeConfiguration<Announcement>
{
	public void Configure(EntityTypeBuilder<Announcement> builder)
	{
		builder.ToTable("announcements");

		builder.Property(c => c.Id)
			.ValueGeneratedOnAdd()
			.IsRequired();

		builder.Property(c => c.Title)
			.HasMaxLength(50)
			.IsRequired();

		builder.Property(c => c.Text)
			.HasMaxLength(1000)
			.IsRequired(false);

		builder.Property(c => c.CreatedAt)
			.IsRequired();
	}
}
