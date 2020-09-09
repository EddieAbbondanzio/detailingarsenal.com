using System;

namespace DetailingArsenal.Application.Users.Security {
    public class RemoveRoleFromUserCommand : IAction {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}