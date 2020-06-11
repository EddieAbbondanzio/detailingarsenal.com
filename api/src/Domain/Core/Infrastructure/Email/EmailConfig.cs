namespace DetailingArsenal.Domain {
    public class EmailConfig {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string SMTP { get; set; } = null!;
        public int Port { get; set; }
    }
}