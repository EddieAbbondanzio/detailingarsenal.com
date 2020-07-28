using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Billing {
    public class SubscriptionPlan : Aggregate<SubscriptionPlan>, IBillingEntity {
        public string Name { get; set; }
        public string? Description { get; set; }
        public BillingReference BillingReference { get; }
        public List<SubscriptionPlanPrice> Prices { get; set; }

        public SubscriptionPlan(string name, string? description, BillingReference billingReference, List<SubscriptionPlanPrice> prices) {
            Name = name;
            Description = description;
            BillingReference = billingReference;
            Prices = prices;
        }
    }
}