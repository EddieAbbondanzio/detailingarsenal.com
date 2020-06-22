using System;

namespace DetailingArsenal.Application.Security {
    public class DeletePermissionCommand : IAction {
        public Guid Id { get; set; }
    }
}