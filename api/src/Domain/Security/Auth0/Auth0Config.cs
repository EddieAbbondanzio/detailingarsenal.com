namespace DetailingArsenal.Domain {
    public class Auth0Config {
        public string Domain { get; set; } = null!;
        public string Identifier { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
    }
}