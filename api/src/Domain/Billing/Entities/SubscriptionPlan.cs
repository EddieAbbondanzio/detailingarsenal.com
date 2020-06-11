using System.Collections.Generic;

namespace DetailingArsenal.Domain {
    public class SubscriptionPlan : Entity<SubscriptionPlan> {
        public string Name { get; set; } = null!;
        public string ExternalId { get; set; } = null!;
        public List<SubscriptionPlanPrice> Prices { get; set; } = new List<SubscriptionPlanPrice>();
    }
}