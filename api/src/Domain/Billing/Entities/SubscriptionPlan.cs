using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Billing {
    public class SubscriptionPlan : Aggregate<SubscriptionPlan>, IBillingEntity {
        public string Name { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public BillingReference BillingReference { get; set; } = null!;
        public List<SubscriptionPlanPrice> Prices { get; set; } = new List<SubscriptionPlanPrice>();
    }
}