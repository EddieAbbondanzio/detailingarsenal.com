using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Persistence.Billing {
    public class SubscriptionPlanModel : IDataTransferObject {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid BillingReferenceId { get; set; }
        public Guid? RoleId { get; set; }
    }
}