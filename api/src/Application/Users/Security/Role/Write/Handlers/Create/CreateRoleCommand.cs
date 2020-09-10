using System;
using System.Collections.Generic;

namespace DetailingArsenal.Application.Users.Security {
    public class CreateRoleCommand : IAction {
        public string Name { get; set; } = null!;
        public List<Guid> PermissionIds { get; set; } = new List<Guid>();
    }
}