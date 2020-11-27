using System;

namespace DetailingArsenal.Application.Users.Security {
    public class UpdatePermissionCommand : IAction {
        public Guid Id { get; set; }
        public string Action { get; set; } = null!;
        public string Scope { get; set; } = null!;
    }
}