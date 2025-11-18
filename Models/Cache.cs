namespace MonitoreoMultifuente3.Models
{
    public class Cache
    {
        public required string key_varChar { get; set; }
        public required string value_mediumtext { get; set; }
        public int expiration_int { get; set; }
    }
}
    
