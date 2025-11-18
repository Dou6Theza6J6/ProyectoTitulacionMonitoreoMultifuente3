using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonitoreoMultifuente3.Components.Account;
using MonitoreoMultifuente3.Data;
using MonitoreoMultifuente3.Hubs;
using MonitoreoMultifuente3.Models;
using MonitoreoMultifuente3.Services;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace MonitoreoMultifuente3
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }
        public static IConfiguration Configuration { get; private set; }

        static App()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(System.AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public App()
        {
            ExcelPackage.License.SetNonCommercialPersonal("ITSVA");
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    // Esta es la línea que YA TIENES
                    /*services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        var connectionString = Configuration.GetConnectionString("DefaultConnection");
                        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                    });
                    */
                    // --- ESTA ES LA LÍNEA QUE FALTA ---
                    // Agrega esta "fábrica" de DbContext. Esto es lo que soluciona
                    // el error 'A second operation was started...' al ser "thread-safe".
                    services.AddDbContextFactory<ApplicationDbContext>(options =>
{
    var connectionString = Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
                    // --- FIN DE LA LÍNEA QUE FALTA ---

                    services.AddWpfBlazorWebView();
                    services.AddAuthorizationCore();

                    // --- Registros de Autenticación Corregidos ---
                    services.AddScoped<DesktopAuthenticationStateProvider>();
                    services.AddScoped<AuthenticationStateProvider>(sp =>
                        sp.GetRequiredService<DesktopAuthenticationStateProvider>());
                    services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
                    // --- Fin de Registros de Auth ---

                    services.AddSingleton<SensorDataService>();
                    services.AddSignalR();
                    services.AddAuthentication();

                    services.AddIdentity<ApplicationUser, IdentityRole<int>>(options => {
                        options.Password.RequireDigit = false;
                        options.Password.RequiredLength = 4;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireLowercase = false;
                    })
                        .AddEntityFrameworkStores<ApplicationDbContext>()
                        .AddSignInManager()
                        .AddDefaultTokenProviders()
                        .AddRoles<IdentityRole<int>>();

                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<SensorStatusService>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            await AppHost!.StartAsync();
            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}