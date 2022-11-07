using EmployeeRecordSystem.Data.Helpers;
using EmployeeRecordSystem.Server.Installers.Helpers;
using EmployeeRecordSystem.Server.Middlewares;

namespace EmployeeRecordSystem.Server.Installers;

public class MiscellaneousInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DatabaseSeeder>();
        services.AddScoped<ErrorHandlingMiddleware>();
    }
}