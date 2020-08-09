using System;

namespace DetailingArsenal.Domain.Billing {
    public class Subscription : Entity<Subscription>, IBillingEntity {
        public string Status { get; set; }
        public Period TrialPeriod { get; set; }
        public Period Period { get; set; }
        public bool CancellingAtPeriodEnd { get; set; }
        public SubscriptionPlanReference PlanReference { get; }
        public BillingReference BillingReference { get; }

        public Subscription(string status, Period trialPeriod, Period period, bool cancellingAtPeriodEnd, SubscriptionPlanReference planReference, BillingReference billingReference) {
            Id = Guid.NewGuid();
            Status = status;
            TrialPeriod = trialPeriod;
            Period = period;
            CancellingAtPeriodEnd = cancellingAtPeriodEnd;
            PlanReference = planReference;
            BillingReference = billingReference;
        }

        public Subscription(Guid id, string status, Period trialPeriod, Period period, bool cancellingAtPeriodEnd, SubscriptionPlanReference planReference, BillingReference billingReference) {
            Id = id;
            Status = status;
            TrialPeriod = trialPeriod;
            Period = period;
            CancellingAtPeriodEnd = cancellingAtPeriodEnd;
            PlanReference = planReference;
            BillingReference = billingReference;
        }
    }
}