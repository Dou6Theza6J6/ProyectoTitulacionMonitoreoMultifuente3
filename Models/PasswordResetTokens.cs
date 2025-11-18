namespace MonitoreoMultifuente3.Models
{
    public class PasswordResetTokens
    {
        public required string email_varChar { get; set; }
        public required string token_varChar { get; set; }
        public DateTime created_at_timestamp { get; set; }
    }
}
