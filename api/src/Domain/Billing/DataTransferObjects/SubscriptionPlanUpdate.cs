using System;

namespace DetailingArsenal.Domain.Billing {
    public class SubscriptionPlanUpdate : IDataTransferObject {
        public Guid RoleId { get; set; }

        public SubscriptionPlanUpdate(Guid roleId) {
            RoleId = roleId;
        }
    }
}