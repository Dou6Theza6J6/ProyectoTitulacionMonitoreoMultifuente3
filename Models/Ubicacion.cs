using System.ComponentModel.DataAnnotations;

namespace MonitoreoMultifuente3.Models
{
    public class Ubicacion
    {
        [Key]
        public int ubicacion_id { get; set; }
        // CORREGIDO: El nombre ahora coincide con el que usas en el formulario.
        public string? nombre { get; set; }
        // CORREGIDO: Cambiado a double para ser compatible con el mapa.
        public double latitud { get; set; }
        public double longitud { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        // CORREGIDO: Se inicializa la colección para evitar errores de "required".
        public virtual ICollection<Escenario> Escenarios { get; set; } = new List<Escenario>();
    }
}