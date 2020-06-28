using System;

namespace DetailingArsenal.Domain.Billing {
    public class SubscriptionPlanPrice : Entity<SubscriptionPlanPrice> {
        public decimal Price { get; set; }
        public string Interval { get; set; } = null!;
        public string ExternalId { get; set; } = null!;

        public static SubscriptionPlanPrice Create(string externalId, decimal price, string interval) {
            return new SubscriptionPlanPrice() {
                Id = Guid.NewGuid(),
                ExternalId = externalId,
                Price = price,
                Interval = interval
            };
        }
    }
}