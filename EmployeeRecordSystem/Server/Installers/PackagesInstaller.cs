using System.Reflection;
using EmployeeRecordSystem.Server.Installers.Helpers;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace EmployeeRecordSystem.Server.Installers;

public class PackagesInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(Program).Assembly);

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Employee record system"
            });

            c.MapType<TimeSpan>(() => new OpenApiSchema
            {
                Type = "string",
                Example = new OpenApiString("0.00:00:00")
            });

            // Set the comments path for the Swagger JSON and UI.
            string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        services.AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Program>());
    }
}