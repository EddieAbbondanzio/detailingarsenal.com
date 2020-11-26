using System;

namespace DetailingArsenal.Domain.Scheduling.Billing {
    public class SubscriptionPlanUpdateCommand : IAction {
        public Guid Id { get; }
        public string? Description { get; }
        public Guid? RoleId { get; }

        public SubscriptionPlanUpdateCommand(Guid id, string? description, Guid? roleId) {
            Id = id;
            Description = description;
            RoleId = roleId;
        }
    }
}