using EmployeeRecordSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRecordSystem.Server.Installers;

public class DatabaseInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddDatabaseDeveloperPageExceptionFilter();
    }
}