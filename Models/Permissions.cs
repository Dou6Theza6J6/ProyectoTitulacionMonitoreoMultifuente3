namespace MonitoreoMultifuente3.Models
{
    public class Permissions
    {
        public int id_int { get; set; }
        public required string name_varChar { get; set; }
        public required string slug_varChar { get; set; }
        public DateTime created_at_timestamp { get; set; }
        public DateTime updated_at_timestamp { get; set; }
    }
}
