using System;

namespace DetailingArsenal.Domain.Billing {
    public class Subscription : Entity<Subscription>, IBillingEntity {
        public string Status { get; set; }
        public DateTime? NextPayment { get; set; }
        public DateTime TrialStart { get; set; }
        public DateTime TrialEnd { get; set; }
        public bool CancellingAtPeriodEnd { get; set; }
        public SubscriptionPlanReference PlanReference { get; }
        public BillingReference BillingReference { get; }

        public Subscription(Guid id, string status, DateTime? nextPayment, DateTime trialStart, DateTime trialEnd, bool cancellingAtPeriodEnd, SubscriptionPlanReference planReference, BillingReference billingReference) {
            Id = id;
            Status = status;
            NextPayment = nextPayment;
            TrialStart = trialStart;
            TrialEnd = trialEnd;
            CancellingAtPeriodEnd = cancellingAtPeriodEnd;
            PlanReference = planReference;
            BillingReference = billingReference;
        }

        public Subscription(string status, DateTime? nextPayment, DateTime trialStart, DateTime trialEnd, bool cancellingAtPeriodEnd, SubscriptionPlanReference planReference, BillingReference billingReference) {
            Id = Guid.NewGuid();
            Status = status;
            NextPayment = nextPayment;
            TrialStart = trialStart;
            TrialEnd = trialEnd;
            CancellingAtPeriodEnd = cancellingAtPeriodEnd;
            PlanReference = planReference;
            BillingReference = billingReference;
        }
    }
}