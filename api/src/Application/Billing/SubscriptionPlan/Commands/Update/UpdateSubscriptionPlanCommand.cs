using System;

namespace DetailingArsenal.Application.Billing {
    public class UpdateSubscriptionPlanCommand : IAction {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
    }
}