using EmployeeRecordSystem.Client;
using EmployeeRecordSystem.Client.HttpServices;
using EmployeeRecordSystem.Client.Navigation;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("EmployeeRecordSystem.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("EmployeeRecordSystem.ServerAPI"));

builder.Services.AddApiAuthorization();
builder.Services.AddMudServices();

builder.Services.AddScoped<EmployeeHttpService>();
builder.Services.AddScoped<WithdrawalRequestHttpService>();
builder.Services.AddScoped<GroupHttpService>();
builder.Services.AddScoped<BreadcrumbState>();

await builder.Build().RunAsync();
