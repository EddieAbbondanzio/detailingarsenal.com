using System;

namespace DetailingArsenal.Application.Security {
    public class DeleteRoleCommand : IAction {
        public Guid Id { get; set; }
    }
}