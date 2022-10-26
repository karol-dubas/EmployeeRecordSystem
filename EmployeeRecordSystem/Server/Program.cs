using EmployeeRecordSystem.Data.Helpers;
using EmployeeRecordSystem.Server.Installers;
using EmployeeRecordSystem.Server.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Find and add all services to the container.
var installers = typeof(Program)
	.Assembly
	.ExportedTypes
	.Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
	.Select(Activator.CreateInstance)
	.Cast<IInstaller>()
	.ToList();

installers.ForEach(i => i.InstallServices(builder.Services, builder.Configuration));

var app = builder.Build();

var databaseSeeder = app.Services
	.CreateScope()
	.ServiceProvider
	.GetRequiredService<DatabaseSeeder>();

await databaseSeeder.EnsureDatabaseCreated()
	.ApplyPendingMigrations()
	.SeedAsync();

if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
	app.UseWebAssemblyDebugging();

	app.UseSwagger();
	app.UseSwaggerUI();
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

// Make the Program class public explicitly, so test projects can access it
namespace EmployeeRecordSystem.Server
{
	public class Program { }
}
