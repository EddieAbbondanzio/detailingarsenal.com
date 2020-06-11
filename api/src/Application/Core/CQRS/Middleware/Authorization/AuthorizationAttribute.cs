namespace DetailingArsenal.Application {
    public class AuthorizationAttribute : System.Attribute {
        public string? Action { get; set; }
        public string? Scope { get; set; }
    }
}