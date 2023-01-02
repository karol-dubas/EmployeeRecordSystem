using Duende.IdentityServer.EntityFramework.Options;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EmployeeRecordSystem.Data;

public class ApplicationDbContext : ApiAuthorizationDbContext<Employee, ApplicationRole, Guid>
{
	public ApplicationDbContext(
		DbContextOptions options,
		IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions) { }

	public virtual DbSet<Group> Groups { get; set; }
	public virtual DbSet<EmployeeBilling> EmployeeBillings { get; set; }
	public virtual DbSet<BalanceLog> BalanceLogs { get; set; }
	public virtual DbSet<WithdrawalRequest> WithdrawalRequests { get; set; }
	public virtual DbSet<WithdrawalRequestStatusType> WithdrawalRequestStatuses { get; set; }
	
	public virtual DbSet<Announcement> Announcements { get; set; }
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

		base.OnModelCreating(modelBuilder);
	}
}
