public class AuthorizationAttribute : System.Attribute {
    public string Action { get; set; } = null!;
    public string Scope { get; set; } = null!;
}