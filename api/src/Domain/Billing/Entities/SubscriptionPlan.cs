using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain {
    public class SubscriptionPlan : Aggregate<SubscriptionPlan> {
        public string Name { get; set; } = null!;
        public string ExternalId { get; set; } = null!;
        public Guid RoleId { get; set; }
        public List<SubscriptionPlanPrice> Prices { get; set; } = new List<SubscriptionPlanPrice>();
    }
}