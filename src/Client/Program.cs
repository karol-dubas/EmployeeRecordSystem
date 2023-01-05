using EmployeeRecordSystem.Client;
using EmployeeRecordSystem.Client.HttpServices;
using EmployeeRecordSystem.Client.Navigation;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("EmployeeRecordSystem.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("EmployeeRecordSystem.ServerAPI"));

builder.Services.AddApiAuthorization();
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 3000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddScoped<EmployeeHttpService>();
builder.Services.AddScoped<WithdrawalRequestHttpService>();
builder.Services.AddScoped<GroupHttpService>();
builder.Services.AddScoped<RoleHttpService>();
builder.Services.AddScoped<AnnouncementHttpService>();
builder.Services.AddSingleton<BreadcrumbStateContainer>();
builder.Services.AddTransient<BreadcrumbBuilder>();

await builder.Build().RunAsync();
