namespace MonitoreoMultifuente3.Models
{
    public class FailedJobs
    {
        public int id_bigInt { get; set; }
        public required string uuid_varChar { get; set; }
        public required string connection_text { get; set; }
        public required string queue_text { get; set; }
        public required string payload_longtext { get; set; }
        public required string exception_longtext { get; set; }
        public DateTime failed_at_timestamp { get; set; }
    }
}
