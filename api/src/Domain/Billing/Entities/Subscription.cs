using System;

namespace DetailingArsenal.Domain.Billing {
    public class Subscription : Entity<Subscription>, IBillingEntity {
        public SubscriptionPlanReference PlanReference { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime? NextPayment { get; set; }
        public DateTime TrialStart { get; set; }
        public DateTime TrialEnd { get; set; }
        public BillingReference BillingReference { get; set; } = null!;

        public static Subscription Create(Guid planId, string priceBillingId, string status, DateTime trialStart, DateTime trialEnd, BillingReference billingReference, DateTime? nextPayment) {
            return new Subscription() {
                Id = Guid.NewGuid(),
                PlanReference = new SubscriptionPlanReference(
                    planId, priceBillingId
                ),
                Status = status,
                TrialStart = trialStart,
                TrialEnd = trialEnd,
                BillingReference = billingReference,
                NextPayment = nextPayment
            };
        }
    }
}