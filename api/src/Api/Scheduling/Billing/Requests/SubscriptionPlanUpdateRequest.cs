using System;

namespace DetailingArsenal.Api.Scheduling.Billing {
    public class SubscriptionPlanUpdateRequest : IDataTransferObject {
        public string? Description { get; set; }
        public Guid? RoleId { get; set; }
    }
}