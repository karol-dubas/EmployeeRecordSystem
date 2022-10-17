using EmployeeRecordSystem.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static EmployeeRecordSystem.Server.Installers.ServiceAttributes;

namespace EmployeeRecordSystem.Server.Installers
{
    public static class ServiceInstaller
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            Type scopedRegistration = typeof(ScopedRegistrationAttribute);
            Type singletonRegistration = typeof(SingletonRegistrationAttribute);
            Type transientRegistration = typeof(TransientRegistrationAttribute);

            var types = typeof(Program)
                .Assembly
                .GetTypes()
                .Where(t => t.IsDefined(scopedRegistration, false)
                || t.IsDefined(transientRegistration, false)
                || t.IsDefined(singletonRegistration, false)
                && !t.IsInterface
                && !t.IsAbstract
                && t.IsSubclassOf(typeof(BaseService)))
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
}
