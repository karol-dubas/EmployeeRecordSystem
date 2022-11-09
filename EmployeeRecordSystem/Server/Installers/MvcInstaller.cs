using EmployeeRecordSystem.Server.ErrorHandlers;
using EmployeeRecordSystem.Server.Installers.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeRecordSystem.Server.Installers;

public class MvcInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        
        services.AddControllersWithViews(o => o.Filters.Add<ValidationFilter>());
        services.AddRazorPages();
    }
}