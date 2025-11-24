using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace MonitoreoMultifuente3.Models
{
    // Esta clase hereda de IdentityUser para ser compatible con ASP.NET Core Identity.
    public class ApplicationUser : IdentityUser<int>
    {
        // Las propiedades personalizadas para el modelo de usuario.
        public required string name_varChar { get; set; }
        public string? remember_token_varChar { get; set; }

        // El resto de las propiedades del modelo User (como Id, Email, etc.)
        // son gestionadas internamente por la clase base IdentityUser<int>.

        // Relaciones con otras tablas
        [InverseProperty("User")]
        public virtual ICollection<Medicion> Mediciones { get; set; } = new List<Medicion>();

        // El resto de las propiedades del modelo User (como Id, Email, etc.)
        // son gestionadas internamente por la clase base IdentityUser<int>.

        // Constructor para inicializar las colecciones
        public ApplicationUser()
        {
            Mediciones = new HashSet<Medicion>();
        }
    }
}
