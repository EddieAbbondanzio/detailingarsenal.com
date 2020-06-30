using System;

namespace DetailingArsenal.Domain.Billing {
    public class UpdateSubscriptionPlan : IDataTransferObject {
        public Guid RoleId { get; set; }

        public UpdateSubscriptionPlan(Guid roleId) {
            RoleId = roleId;
        }
    }
}