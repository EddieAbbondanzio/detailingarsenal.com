using System.Collections.Generic;

namespace DetailingArsenal.Domain {
    public class ExternalSubscriptionPlan : ValueObject<ExternalSubscriptionPlan> {
        public string Name { get; }
        public string ExternalId { get; }
        public List<ExternalSubscriptionPlanPrice> Prices { get; }

        public ExternalSubscriptionPlan(string name, string externalId, List<ExternalSubscriptionPlanPrice> prices) {
            Name = name;
            Prices = prices;
            ExternalId = externalId;
        }
    }
}