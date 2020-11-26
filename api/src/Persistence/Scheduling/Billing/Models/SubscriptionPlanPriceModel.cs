using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Persistence.Scheduling.Billing {
    public class SubscriptionPlanPriceModel : IDataTransferObject {
        public Guid Id { get; set; }
        public Guid BillingReferenceId { get; set; }
        public Guid PlanId { get; set; }
        public decimal Price { get; set; }
        public string Interval { get; set; } = null!;
    }
}