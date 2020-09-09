using System;

namespace DetailingArsenal.Application.Users.Security {
    public class DeleteRoleCommand : IAction {
        public Guid Id { get; set; }
    }
}