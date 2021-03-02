using System;
using System.Collections.Generic;

namespace DetailingArsenal.Application.Users.Security {
    public record RoleCreateCommand(string Name, List<Guid> PermissionIds) : IAction;
}