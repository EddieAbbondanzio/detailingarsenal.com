namespace DetailingArsenal.Domain {
    public class Permission : Aggregate<Permission> {
        public const int ActionMaxLength = 32;
        public const int ScopeMaxLength = 32;


        public string Action { get; set; } = null!;
        public string Scope { get; set; } = null!;

        public override string? ToString() {
            return $"{Action}:{Scope}";
        }
    }
}