using Duende.IdentityServer.EntityFramework.Options;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Data.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EmployeeRecordSystem.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<Employee, ApplicationRole, Guid>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<EmployeeBilling> EmployeeBillings { get; set; }
        public DbSet<BalanceLog> BalanceLogs { get; set; }
        public DbSet<WithdrawalRequest> WithdrawalRequests { get; set; }
        public DbSet<WithdrawalRequestStatusType> WithdrawalRequestStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}