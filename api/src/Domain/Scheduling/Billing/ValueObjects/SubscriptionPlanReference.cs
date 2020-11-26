using System;

namespace DetailingArsenal.Domain.Scheduling.Billing {
    public class SubscriptionPlanReference : ValueObject<SubscriptionPlanReference> {
        public Guid PlanId { get; }
        public string PriceBillingId { get; }

        public SubscriptionPlanReference(Guid planId, string priceBillingId) {
            PlanId = planId;
            PriceBillingId = priceBillingId;
        }
    }
}