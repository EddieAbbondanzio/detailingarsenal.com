using System;

namespace DetailingArsenal.Application.Users.Security {
    public record PermissionReadModel(Guid Id, string Action, string Scope) : IAction;
}