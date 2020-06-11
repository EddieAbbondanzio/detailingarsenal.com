using System;

namespace DetailingArsenal.Application {
    public class RemoveRoleFromUserCommand : IAction {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}