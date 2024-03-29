﻿using EmployeeRecordSystem.Data.Helpers;
using EmployeeRecordSystem.Server.ErrorHandlers;
using EmployeeRecordSystem.Server.Installers.Helpers;

namespace EmployeeRecordSystem.Server.Installers;

public class MiscellaneousInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<InitAdminConfiguration>()
            .Bind(configuration.GetSection(InitAdminConfiguration.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddScoped<DatabaseSeeder>();
        services.AddScoped<ErrorHandlingMiddleware>();
    }
}