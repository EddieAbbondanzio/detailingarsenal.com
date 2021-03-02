using System;
using System.Collections.Generic;

namespace DetailingArsenal.Application.Users.Security {
    public record RoleUpdateCommand(Guid Id, string Name, List<Guid> PermissionIds) : IAction;
}