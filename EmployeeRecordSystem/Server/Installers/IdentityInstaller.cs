using System.IdentityModel.Tokens.Jwt;
using EmployeeRecordSystem.Data;
using EmployeeRecordSystem.Data.Entities;
using Microsoft.AspNetCore.Authentication;

namespace EmployeeRecordSystem.Server.Installers;

public class IdentityInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDefaultIdentity<Employee>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.User.RequireUniqueEmail = true;
            }) 
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        services.AddIdentityServer()
            .AddApiAuthorization<Employee, ApplicationDbContext>(options =>
            {
                // Add the role claim to employee claims collection:

                // For Identity resources
                options.IdentityResources["openid"].UserClaims.Add("role");

                // And for API resources
                options.ApiResources.Single().UserClaims.Add("role");
            });
        
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");
        
        services.AddAuthentication()
            .AddIdentityServerJwt();
    }
}