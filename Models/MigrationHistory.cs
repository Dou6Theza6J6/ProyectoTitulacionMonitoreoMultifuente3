namespace MonitoreoMultifuente3.Models
{
    public class MigrationHistory
    {
        public int id_int { get; set; }
        public required string migration_varChar { get; set; }
        public int batch_int { get; set; }
    }
}
