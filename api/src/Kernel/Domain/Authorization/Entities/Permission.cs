public class Permission : Entity<Permission> {
    public string Action { get; set; } = null!;
    public string Scope { get; set; } = null!;

    public override string? ToString() {
        return $"{Action}:{Scope}";
    }
}