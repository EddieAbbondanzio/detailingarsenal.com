using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Persistence.Billing {
    public class SubscriptionModel : IDataTransferObject {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid PlanId { get; set; }
        public Guid BillingReferenceId { get; set; }
        public string Status { get; set; } = null!;
    }
}