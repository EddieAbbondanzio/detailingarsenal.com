using System;

namespace DetailingArsenal.Application.Users.Security {
    public record AddRoleToUserCommand(Guid UserId, Guid RoleId) : IAction;
}