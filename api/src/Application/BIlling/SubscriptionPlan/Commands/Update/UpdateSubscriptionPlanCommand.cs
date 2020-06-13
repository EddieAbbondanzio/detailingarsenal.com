using System;

namespace DetailingArsenal.Application {
    public class UpdateSubscriptionPlanCommand : IAction {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
    }
}