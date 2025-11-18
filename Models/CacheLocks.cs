namespace MonitoreoMultifuente3.Models
{
    public class CacheLocks
    {
        public required string key_varChar { get; set; }
        public required string owner_varChar { get; set; }
        public int expiration_int { get; set; }
    }
}
