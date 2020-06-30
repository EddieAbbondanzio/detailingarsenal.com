using System;

namespace DetailingArsenal.Domain.Billing {
    public class SubscriptionPlanPrice : ValueObject<SubscriptionPlanPrice> {
        public decimal Price { get; }
        public string Interval { get; }
        public BillingReference BillingReference { get; }

        public SubscriptionPlanPrice(decimal price, string interval, BillingReference billingReference) {
            Price = price;
            Interval = interval;
            BillingReference = billingReference;
        }
    }
}