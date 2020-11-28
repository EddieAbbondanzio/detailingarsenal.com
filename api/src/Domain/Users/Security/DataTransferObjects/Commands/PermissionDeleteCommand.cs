using System;

namespace DetailingArsenal.Application.Users.Security {
    public record PermissionDeleteCommand(Guid Id) : IAction;
}