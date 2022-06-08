using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Data
{
    public class DatabaseSeeder
    {
        private readonly ApplicationDbContext _dbContext;

        public DatabaseSeeder(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        /// <summary>
        /// Create the database and migrate if it doesn't exist
        /// </summary>
        public DatabaseSeeder EnsureDatabaseCreated()
{
            if (!_dbContext.Database.CanConnect())
{
                RelationalDatabaseFacadeExtensions.Migrate(_dbContext.Database);
            }

            return this;
        }

        public DatabaseSeeder ApplyPendingMigrations()
{
            var pendingMigrations = _dbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                _dbContext.Database.Migrate();
            }

            return this;
        }

        public DatabaseSeeder Seed()
        {
            //SeedAdmin();
            //SeedRoles(); // role vs. position?

            return this;
        }
    }
}
