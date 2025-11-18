namespace MonitoreoMultifuente3.Models
{
    public class Sessions
    {
        public required string id_varChar { get; set; }
        public int user_id_bigInt { get; set; }
        public required string ip_address_varChar { get; set; }
        public required string user_agent_text { get; set; }
        public required string payload_longtext { get; set; }
        public int last_activity_int { get; set; }

        // Relación
        public virtual required ApplicationUser User { get; set; }
    }
}
