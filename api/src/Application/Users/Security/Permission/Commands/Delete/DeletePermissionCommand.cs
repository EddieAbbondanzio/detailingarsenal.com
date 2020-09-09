using System;

namespace DetailingArsenal.Application.Users.Security {
    public class DeletePermissionCommand : IAction {
        public Guid Id { get; set; }
    }
}