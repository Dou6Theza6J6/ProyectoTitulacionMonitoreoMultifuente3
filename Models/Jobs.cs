namespace MonitoreoMultifuente3.Models
{
    public class Jobs
    {
        public int id_bigInt { get; set; }
        public required string queue_varChar { get; set; }
        public required string payload_longtext { get; set; }
        public int attempts_tinyInt { get; set; }
        public int reserved_at_int { get; set; }
        public int available_at_int { get; set; }
        public DateTime created_at_int { get; set; }
    }
}
