using System;

namespace DetailingArsenal.Domain.Users.Security {
    public class Permission : Aggregate<Permission> {
        public const int ActionMaxLength = 32;
        public const int ScopeMaxLength = 32;

        public string Action { get; set; } = null!;
        public string Scope { get; set; } = null!;

        public Permission(string action, string scope) {
            Id = Guid.NewGuid();
            Action = action;
            Scope = scope;
        }

        public Permission(Guid id, string action, string scope) {
            Id = id;
            Action = action;
            Scope = scope;
        }

        public override string? ToString() {
            return $"{Action}:{Scope}";
        }
    }
}