using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Billing {
    public class SubscriptionPlan : Aggregate<SubscriptionPlan> {
        public string Name { get; set; } = null!;
        public BillingReference BillingReference { get; set; } = null!;
        public Guid? RoleId { get; set; }
        public List<SubscriptionPlanPrice> Prices { get; set; } = new List<SubscriptionPlanPrice>();
    }
}