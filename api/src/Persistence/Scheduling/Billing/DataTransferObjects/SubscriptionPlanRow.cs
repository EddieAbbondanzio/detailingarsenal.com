using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Persistence.Scheduling.Billing {
    public class SubscriptionPlanRow : IDataTransferObject {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid BillingReferenceId { get; set; }
        public Guid? RoleId { get; set; }
    }
}