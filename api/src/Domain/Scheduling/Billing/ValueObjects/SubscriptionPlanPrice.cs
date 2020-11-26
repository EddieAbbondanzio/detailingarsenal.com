using System;

namespace DetailingArsenal.Domain.Scheduling.Billing {
    public class SubscriptionPlanPrice : ValueObject<SubscriptionPlanPrice> {
        public decimal Amount { get; }
        public string Interval { get; }
        public BillingReference BillingReference { get; }

        public SubscriptionPlanPrice(decimal amount, string interval, BillingReference billingReference) {
            Amount = amount;
            Interval = interval;
            BillingReference = billingReference;
        }
    }
}