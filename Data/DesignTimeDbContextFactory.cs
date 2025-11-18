using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MonitoreoMultifuente3.Data
{
    /*
     * Esta clase es una "fábrica" que solo usan las herramientas de diseño de EF Core (como Add-Migration).
     * Proporciona una forma de crear el DbContext leyendo 'appsettings.json'
     * sin necesidad de construir e iniciar toda la aplicación WPF (que es lo que fallaba).
     */
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Busca 'appsettings.json' en el directorio del proyecto
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Obtiene la cadena de conexión
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Configura las opciones del DbContext
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            // Devuelve un nuevo DbContext
            return new ApplicationDbContext(builder.Options);
        }
    }
}
