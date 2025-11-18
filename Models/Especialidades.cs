namespace MonitoreoMultifuente3.Models
{
    public class Especialidades
    {
        public int id_int { get; set; }
        public required string nombre_varChar { get; set; }

        // Relación
        public virtual required ICollection<Sensor> Sensores { get; set; }
    }
}
