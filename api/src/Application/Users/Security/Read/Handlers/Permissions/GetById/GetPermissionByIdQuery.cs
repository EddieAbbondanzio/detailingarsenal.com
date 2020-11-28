using System;

namespace DetailingArsenal.Application.Users.Security {
    public record GetPermissionByIdQuery(Guid Id) : IAction;
}