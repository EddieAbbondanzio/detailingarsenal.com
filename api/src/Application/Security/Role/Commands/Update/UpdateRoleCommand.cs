using System;
using System.Collections.Generic;

namespace DetailingArsenal.Application.Security {
    public class UpdateRoleCommand : IAction {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Guid> PermissionIds { get; set; } = new List<Guid>();
    }
}