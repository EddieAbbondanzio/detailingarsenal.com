using System;

namespace DetailingArsenal.Domain.Scheduling.Billing {
    public class Subscription : Entity<Subscription>, IBillingEntity {
        public SubscriptionStatus Status { get; set; }
        public Period TrialPeriod { get; set; }
        public Period Period { get; set; }
        public bool CancellingAtPeriodEnd { get; set; }
        public SubscriptionPlanReference PlanReference { get; }
        public BillingReference BillingReference { get; }

        public Subscription(SubscriptionStatus status, Period trialPeriod, Period period, bool cancellingAtPeriodEnd, SubscriptionPlanReference planReference, BillingReference billingReference) {
            Id = Guid.NewGuid();
            Status = status;
            TrialPeriod = trialPeriod;
            Period = period;
            CancellingAtPeriodEnd = cancellingAtPeriodEnd;
            PlanReference = planReference;
            BillingReference = billingReference;
        }

        public Subscription(Guid id, SubscriptionStatus status, Period trialPeriod, Period period, bool cancellingAtPeriodEnd, SubscriptionPlanReference planReference, BillingReference billingReference) {
            Id = id;
            Status = status;
            TrialPeriod = trialPeriod;
            Period = period;
            CancellingAtPeriodEnd = cancellingAtPeriodEnd;
            PlanReference = planReference;
            BillingReference = billingReference;
        }

        // I don't like this here but it feels like an orphan.
        public static SubscriptionStatus ParseStatus(string status) => status switch {
            "trialing" => SubscriptionStatus.Trialing,
            "active" => SubscriptionStatus.Active,
            "incomplete" => SubscriptionStatus.Incomplete,
            "incomplete_expired" => SubscriptionStatus.IncompleteExpired,
            "past_due" => SubscriptionStatus.PastDue,
            "canceled" => SubscriptionStatus.Canceled,
            "unpaid" => SubscriptionStatus.Unpaid,
            _ => throw new NotSupportedException($"Unknown subscription status string of {status} passed.")
        };
    }
}