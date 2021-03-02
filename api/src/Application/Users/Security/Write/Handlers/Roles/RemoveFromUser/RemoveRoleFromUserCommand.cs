using System;

namespace DetailingArsenal.Application.Users.Security {
    public record RemoveRoleFromUserCommand(Guid UserId, Guid RoleId) : IAction;
}