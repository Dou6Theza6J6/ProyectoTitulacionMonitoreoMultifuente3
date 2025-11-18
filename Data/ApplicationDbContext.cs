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

        // Se han eliminado las DbSet de tablas no necesarias, como Alumno, Carrera, etc.
        public DbSet<Escenario> Escenarios { get; set; }
        public DbSet<Medicion> Mediciones { get; set; }
        public DbSet<Parametro> Parametros { get; set; }
        public DbSet<Sensor> Sensores { get; set; }
        public DbSet<Ubicacion> Ubicaciones { get; set; }
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

            // Mapeamos explícitamente las tablas de Identity que se desea mantener.
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

            modelBuilder.Entity<IdentityUserClaim<int>>(entity =>
            {
                entity.ToTable("user_claims");
            });

            modelBuilder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.ToTable("user_tokens");
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<IdentityRoleClaim<int>>(entity =>
            {
                entity.ToTable("role_claims");
            });

            // Mapeo de tablas personalizadas
            modelBuilder.Entity<Escenario>().ToTable("escenarios");
            modelBuilder.Entity<Escenario>().HasKey(e => e.escenario_id);
            modelBuilder.Entity<Escenario>()
                .HasOne(e => e.Ubicacion)
                .WithMany(u => u.Escenarios)
                .HasForeignKey(e => e.ubicacion_id);

            modelBuilder.Entity<Medicion>().ToTable("mediciones");
            modelBuilder.Entity<Medicion>().HasKey(m => m.registro_medicion_id);
            modelBuilder.Entity<Medicion>()
                .HasOne(m => m.User)
                .WithMany(u => u.Mediciones)
                .HasForeignKey(m => m.user_id);
            modelBuilder.Entity<Medicion>()
                .HasOne(m => m.Escenario)
                .WithMany(e => e.Mediciones)
                .HasForeignKey(m => m.escenario_id);
            modelBuilder.Entity<Medicion>()
                .HasOne(m => m.Sensor)
                .WithMany(s => s.Mediciones)
                .HasForeignKey(m => m.sensor_id);
            modelBuilder.Entity<Medicion>()
                .HasOne(m => m.Parametro)
                .WithMany(p => p.Mediciones)
                .HasForeignKey(m => m.parametro_id);

            modelBuilder.Entity<Parametro>().ToTable("parametros");
            modelBuilder.Entity<Parametro>().HasKey(p => p.parametro_id);
            modelBuilder.Entity<Parametro>()
                .HasOne(p => p.Sensor)
                .WithMany(s => s.Parametros)
                .HasForeignKey(p => p.sensor_id);

            modelBuilder.Entity<Sensor>().ToTable("sensores");
         /*  modelBuilder.Entity<Sensor>().HasKey(s => s.sensor_id); */

            modelBuilder.Entity<Ubicacion>().ToTable("ubicaciones");
            modelBuilder.Entity<Ubicacion>().HasKey(u => u.ubicacion_id);

            modelBuilder.Entity<Cache>().ToTable("cache").HasKey(c => c.key_varChar);
            modelBuilder.Entity<CacheLocks>().ToTable("cache_locks").HasKey(c => c.key_varChar);
            modelBuilder.Entity<FailedJobs>().ToTable("failed_jobs").HasKey(f => f.id_bigInt);
            modelBuilder.Entity<Jobs>().ToTable("jobs").HasKey(j => j.id_bigInt);
            modelBuilder.Entity<JobBatches>().ToTable("job_batches").HasKey(j => j.id_varChar);
            modelBuilder.Entity<MigrationHistory>().ToTable("migrations").HasKey(m => m.id_int);
            modelBuilder.Entity<PasswordResetTokens>().ToTable("password_reset_tokens").HasKey(p => p.email_varChar);
            modelBuilder.Entity<Permissions>().ToTable("permissions").HasKey(p => p.id_int);
            modelBuilder.Entity<RolePermission>().ToTable("role_permission").HasKey(rp => rp.id_bigInt);
            modelBuilder.Entity<Sessions>().ToTable("sessions").HasKey(s => s.id_varChar);

            modelBuilder.Entity<Sessions>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.user_id_bigInt);
        }
    }
}
