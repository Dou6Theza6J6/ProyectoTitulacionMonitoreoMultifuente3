namespace MonitoreoMultifuente3.Models
{
    public class JobBatches
    {
        public required string id_varChar { get; set; }
        public required string name_varChar { get; set; }
        public int total_jobs_int { get; set; }
        public int pending_jobs_int { get; set; }
        public int failed_jobs_int { get; set; }
        public required string failed_jobs_ids_longtext { get; set; }
        public required string canceled_at_int { get; set; }
        public DateTime created_at_int { get; set; }
        public int finished_at_int { get; set; }
    }
}
