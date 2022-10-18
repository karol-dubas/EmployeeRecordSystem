using EmployeeRecordSystem.Server.Services;
using static EmployeeRecordSystem.Server.Installers.ServiceAttributes;

namespace EmployeeRecordSystem.Server.Installers;

public class ServiceInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        var scopedRegistration = typeof(ScopedRegistrationAttribute);
        var singletonRegistration = typeof(SingletonRegistrationAttribute);
        var transientRegistration = typeof(TransientRegistrationAttribute);

        var types = typeof(Program)
            .Assembly
            .GetTypes()
            .Where(t => t.IsDefined(scopedRegistration, false)
                        || t.IsDefined(transientRegistration, false)
                        || (t.IsDefined(singletonRegistration, false)
                            && !t.IsInterface
                            && !t.IsAbstract
                            && t.IsSubclassOf(typeof(BaseService))))
            .Select(s => new
            {
                Interface = s.GetInterface($"I{s.Name}"),
                Service = s
            })
            .Where(x => x.Service != null);

        foreach (var type in types)
        {
            if (type.Service.IsDefined(scopedRegistration, false))
                services.AddScoped(type.Interface, type.Service);

            if (type.Service.IsDefined(transientRegistration, false))
                services.AddTransient(type.Interface, type.Service);

            if (type.Service.IsDefined(singletonRegistration, false))
                services.AddSingleton(type.Interface, type.Service);
        }
    }
}