using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Scheduling.Billing {
    public class SubscriptionPlan : Aggregate<SubscriptionPlan>, IBillingEntity {
        public const int DescriptionMaxLength = 1024;

        public string Name { get; set; }
        public string? Description { get; set; }
        public BillingReference BillingReference { get; }
        public List<SubscriptionPlanPrice> Prices { get; set; }

        /// <summary>
        /// Role that will be assigned to each user that has this plan. 
        /// </summary>
        public Guid? RoleId { get; set; }

        public SubscriptionPlan(Guid id, string name, string? description, BillingReference billingReference, List<SubscriptionPlanPrice>? prices = null, Guid? roleId = null) {
            Id = id;
            Name = name;
            Description = description;
            BillingReference = billingReference;
            Prices = prices ?? new List<SubscriptionPlanPrice>();
            RoleId = roleId;
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