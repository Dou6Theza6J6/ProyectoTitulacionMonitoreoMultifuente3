namespace MonitoreoMultifuente3.Models
{
    public class Escenario
    {
        public int escenario_id { get; set; }
        public string? nombre { get; set; }
        // CORREGIDO: El nombre ahora es "descripcion_varChar" para consistencia.
        public string? descripcion { get; set; }

        public string? pais { get; set; }

        public string? ciudad { get; set; }

        public string? calles { get; set; }

        public string? codigo_p { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int ubicacion_id { get; set; }

        public int tipo_e { get; set; }

        // CORREGIDO: Las relaciones ahora pueden ser nulas para evitar errores de "required" al crear el objeto.
        public virtual Ubicacion? Ubicacion { get; set; }
        public virtual ICollection<Medicion> Mediciones { get; set; } = new List<Medicion>();
    }
}