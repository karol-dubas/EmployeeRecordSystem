using Duende.IdentityServer.EntityFramework.Options;
using EmployeeRecordSystem.Data.Entities;
using EmployeeRecordSystem.Data.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EmployeeRecordSystem.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<UserBilling> UserBillings { get; set; }
        public DbSet<UserOperation> UserOperations { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestStatusType> RequestStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}