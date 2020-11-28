using System;

namespace DetailingArsenal.Domain.Users.Security {
    public record AddRoleToUserCommand(Guid UserId, Guid RoleId) : IAction;
}