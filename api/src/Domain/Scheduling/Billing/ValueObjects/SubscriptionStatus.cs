using System;

namespace DetailingArsenal.Domain.Scheduling.Billing {
    /// <summary>
    /// See: https://stripe.com/docs/billing/subscriptions/overview#subscription-statuses
    /// </summary>
    public enum SubscriptionStatus {
        Trialing = 0,
        Active = 1,
        Incomplete = 2,
        IncompleteExpired = 3,
        PastDue = 4,
        Canceled = 5,
        Unpaid = 6
    }

    public static class SubscriptionStatusExts {
        public static string ToBillingString(this SubscriptionStatus status) => status switch {
            SubscriptionStatus.Trialing => "trialing",
            SubscriptionStatus.Active => "active",
            SubscriptionStatus.Incomplete => "incomplete",
            SubscriptionStatus.IncompleteExpired => "incomplete_expired",
            SubscriptionStatus.PastDue => "past_due",
            SubscriptionStatus.Canceled => "canceled",
            SubscriptionStatus.Unpaid => "unpaid",
            _ => throw new NotSupportedException("Unknown subscription status passed")
        };
    }
}