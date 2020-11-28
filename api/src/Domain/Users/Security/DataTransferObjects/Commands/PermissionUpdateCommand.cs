using System;

namespace DetailingArsenal.Application.Users.Security {
    public record PermissionUpdateCommand(Guid Id, string Action, string Scope) : IAction;
}