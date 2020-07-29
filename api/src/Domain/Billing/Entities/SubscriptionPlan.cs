using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Billing {
    public class SubscriptionPlan : Aggregate<SubscriptionPlan>, IBillingEntity {
        public string Name { get; set; }
        public string? Description { get; set; }
        public BillingReference BillingReference { get; }
        public List<SubscriptionPlanPrice> Prices { get; set; }

        public SubscriptionPlan(Guid id, string name, string? description, BillingReference billingReference, List<SubscriptionPlanPrice>? prices = null) {
            Id = id;
            Name = name;
            Description = description;
            BillingReference = billingReference;
            Prices = prices ?? new List<SubscriptionPlanPrice>();
        }

        public SubscriptionPlan(string name, string? description, BillingReference billingReference, List<SubscriptionPlanPrice>? prices = null) {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            BillingReference = billingReference;
            Prices = prices ?? new List<SubscriptionPlanPrice>();
        }
    }
}