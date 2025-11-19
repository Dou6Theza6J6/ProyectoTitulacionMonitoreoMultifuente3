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
using System.Net.Http; // Necesario para HttpClient

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
                    // 1. Configuración de la Base de Datos (Thread-Safe)
                    services.AddDbContextFactory<ApplicationDbContext>(options =>
                    {
                        var connectionString = Configuration.GetConnectionString("DefaultConnection");
                        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                    });

                    // 2. Servicios esenciales de Blazor Hybrid
                    services.AddWpfBlazorWebView();
                    services.AddAuthorizationCore();

                    // --- ESTE ES EL SERVICIO QUE FALTABA ---
                    services.AddHttpClient();
                    // ---------------------------------------

                    // 3. Autenticación y Usuarios
                    services.AddScoped<DesktopAuthenticationStateProvider>();
                    services.AddScoped<AuthenticationStateProvider>(sp =>
                        sp.GetRequiredService<DesktopAuthenticationStateProvider>());
                    services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();

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

                    // 4. Tus Servicios Personalizados (Sensores)
                    services.AddSingleton<SensorDataService>();   // Para leer del Arduino
                    services.AddSingleton<SensorStatusService>(); // Para ver si están activos/inactivos

                    // 5. Otros servicios
                    services.AddSignalR();

                    // 6. Registrar la ventana principal
                    services.AddSingleton<MainWindow>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            await AppHost!.StartAsync();

            // Manejo de excepciones no controladas para que no se cierre la app de golpe
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var ex = (Exception)args.ExceptionObject;
                MessageBox.Show($"Error crítico no controlado: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            };

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