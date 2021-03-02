using System;

namespace DetailingArsenal.Application.Users.Security {
    public record RoleDeleteCommand(Guid Id) : IAction;
}