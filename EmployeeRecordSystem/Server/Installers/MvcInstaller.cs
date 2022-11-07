using EmployeeRecordSystem.Server.Installers.Helpers;
using EmployeeRecordSystem.Server.Middlewares;
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