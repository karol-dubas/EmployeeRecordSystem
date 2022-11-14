namespace EmployeeRecordSystem.Server.Installers.Helpers;

public interface IInstaller
{
    void InstallServices(IServiceCollection services, IConfiguration configuration);
}