using System;

namespace DetailingArsenal.Domain.Billing {
    public class Subscription : Entity<Subscription>, IBillingEntity {
        public SubscriptionPlanReference PlanReference { get; set; } = null!;
        public string Status { get; set; } = null!;
        public BillingReference BillingReference { get; set; } = null!;

        public static Subscription Create(Guid planId, string priceBillingId, string status, BillingReference billingReference) {
            return new Subscription() {
                Id = Guid.NewGuid(),
                PlanReference = new SubscriptionPlanReference(
                    planId, priceBillingId
                ),
                Status = status,
                BillingReference = billingReference
            };
        }
    }
}