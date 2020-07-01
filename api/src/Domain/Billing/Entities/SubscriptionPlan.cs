using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Billing {
    public class SubscriptionPlan : Aggregate<SubscriptionPlan>, IBillingEntity {
        public string Name { get; set; } = null!;
        public BillingReference BillingReference { get; set; } = null!;
        public Guid? RoleId { get; set; }
        public List<SubscriptionPlanPrice> Prices { get; set; } = new List<SubscriptionPlanPrice>();

        public static SubscriptionPlan Create(string name, BillingReference billingReference, Guid? roleId, List<SubscriptionPlanPrice> prices) {
            return new SubscriptionPlan() {
                Id = Guid.NewGuid(),
                Name = name,
                BillingReference = billingReference,
                RoleId = roleId,
                Prices = prices
            };
        }
    }
}