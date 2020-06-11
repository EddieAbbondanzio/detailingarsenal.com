using System;

namespace DetailingArsenal.Application {
    public class DeleteRoleCommand : IAction {
        public Guid Id { get; set; }
    }
}