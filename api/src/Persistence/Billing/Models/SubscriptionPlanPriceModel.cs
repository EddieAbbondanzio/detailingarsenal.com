using System;

namespace DetailingArsenal.Persistence.Billing {
    public class SubscriptionPlanPriceModel {
        public Guid Id { get; set; }
        public string ExternalId { get; set; } = null!;
        public Guid PlanId { get; set; }
        public decimal Price { get; set; }
        public string Interval { get; set; } = null!;
    }
}