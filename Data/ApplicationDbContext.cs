using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MonitoreoMultifuente3.Models;

namespace MonitoreoMultifuente3.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tablas del Sistema de Monitoreo
        public DbSet<Escenario> Escenarios { get; set; }
        public DbSet<Medicion> Mediciones { get; set; }
        public DbSet<Parametro> Parametros { get; set; }
        public DbSet<Sensor> Sensores { get; set; }
        public DbSet<Ubicacion> Ubicaciones { get; set; }

        // Tablas Auxiliares (Cache, Jobs, etc.)
        public DbSet<Cache> Cache { get; set; }
        public DbSet<CacheLocks> CacheLocks { get; set; }
        public DbSet<FailedJobs> FailedJobs { get; set; }
        public DbSet<Jobs> Jobs { get; set; }
        public DbSet<JobBatches> JobBatches { get; set; }
        public DbSet<MigrationHistory> Migrations { get; set; }
        public DbSet<PasswordResetTokens> PasswordResetTokens { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<Sessions> Sessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- 1. Configuración de Identity (Usuarios y Roles) ---
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("users");
                entity.Property(e => e.Id).HasColumnName("id_bigInt");
            });

            modelBuilder.Entity<IdentityRole<int>>(entity =>
            {
                entity.ToTable("roles");
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<IdentityUserRole<int>>(entity =>
            {
                entity.ToTable("role_user");
                entity.HasKey(e => new { e.UserId, e.RoleId });
            });

            modelBuilder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.ToTable("user_logins");
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });

            modelBuilder.Entity<IdentityUserClaim<int>>(entity => entity.ToTable("user_claims"));
            modelBuilder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.ToTable("user_tokens");
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });
            modelBuilder.Entity<IdentityRoleClaim<int>>(entity => entity.ToTable("role_claims"));

            // --- 2. Mapeo de Tablas del Negocio (AQUÍ ESTÁN LAS RELACIONES) ---

            // Ubicación
            modelBuilder.Entity<Ubicacion>()
                .ToTable("ubicaciones")
                .HasKey(u => u.ubicacion_id);

            // Escenario
            modelBuilder.Entity<Escenario>(entity =>
            {
                entity.ToTable("escenarios");
                entity.HasKey(e => e.escenario_id);

                // Relación: Una Ubicación -> Muchos Escenarios
                entity.HasOne(e => e.Ubicacion)
                      .WithMany(u => u.Escenarios)
                      .HasForeignKey(e => e.ubicacion_id)
                      .OnDelete(DeleteBehavior.Cascade); // Si borras ubicación, se van los escenarios
            });

            // Sensor
            modelBuilder.Entity<Sensor>(entity =>
            {
                entity.ToTable("sensores");
                entity.HasKey(s => s.sensor_id); // Descomentado y asegurado
            });

            // Parametro
            modelBuilder.Entity<Parametro>(entity =>
            {
                entity.ToTable("parametros");
                entity.HasKey(p => p.parametro_id);

                // Relación: Un Sensor -> Muchos Parámetros
                entity.HasOne(p => p.Sensor)
                      .WithMany(s => s.Parametros)
                      .HasForeignKey(p => p.sensor_id)
                      .OnDelete(DeleteBehavior.Cascade); // Si borras sensor, se van sus parámetros
            });

            // Medicion (La tabla central con todas las relaciones)
            modelBuilder.Entity<Medicion>(entity =>
            {
                entity.ToTable("mediciones");
                entity.HasKey(m => m.registro_medicion_id);

                // Relación 1: Un Usuario -> Muchas Mediciones
                entity.HasOne(m => m.User)
                      .WithMany(u => u.Mediciones)
                      .HasForeignKey(m => m.user_id)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relación 2: Un Escenario -> Muchas Mediciones
                entity.HasOne(m => m.Escenario)
                      .WithMany(e => e.Mediciones)
                      .HasForeignKey(m => m.escenario_id)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relación 3: Un Sensor -> Muchas Mediciones
                entity.HasOne(m => m.Sensor)
                      .WithMany(s => s.Mediciones)
                      .HasForeignKey(m => m.sensor_id)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relación 4: Un Parámetro -> Muchas Mediciones
                entity.HasOne(m => m.Parametro)
                      .WithMany(p => p.Mediciones)
                      .HasForeignKey(m => m.parametro_id)
                      // Usamos Restrict para evitar errores de borrado cíclico en algunas BD, 
                      // pero Cascade suele funcionar bien en MySQL. Restrict es más seguro.
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // --- 3. Mapeo de Tablas Auxiliares ---
            modelBuilder.Entity<Cache>().ToTable("cache").HasKey(c => c.key_varChar);
            modelBuilder.Entity<CacheLocks>().ToTable("cache_locks").HasKey(c => c.key_varChar);
            modelBuilder.Entity<FailedJobs>().ToTable("failed_jobs").HasKey(f => f.id_bigInt);
            modelBuilder.Entity<Jobs>().ToTable("jobs").HasKey(j => j.id_bigInt);
            modelBuilder.Entity<JobBatches>().ToTable("job_batches").HasKey(j => j.id_varChar);
            modelBuilder.Entity<MigrationHistory>().ToTable("migrations").HasKey(m => m.id_int);
            modelBuilder.Entity<PasswordResetTokens>().ToTable("password_reset_tokens").HasKey(p => p.email_varChar);
            modelBuilder.Entity<Permissions>().ToTable("permissions").HasKey(p => p.id_int);
            modelBuilder.Entity<RolePermission>().ToTable("role_permission").HasKey(rp => rp.id_bigInt);

            modelBuilder.Entity<Sessions>(entity =>
            {
                entity.ToTable("sessions");
                entity.HasKey(s => s.id_varChar);
                entity.HasOne(s => s.User)
                      .WithMany()
                      .HasForeignKey(s => s.user_id_bigInt);
            });
        }
    }
}