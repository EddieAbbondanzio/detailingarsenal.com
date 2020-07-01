using System;

namespace DetailingArsenal.Domain.Billing {
    public class Subscription : Entity<Subscription> {
        public Guid PlanId { get; set; }
        public string Status { get; set; } = null!;
        public BillingReference BillingReference { get; set; } = null!;

        public static Subscription Create(Guid plainId, string status, BillingReference billingReference) {
            return new Subscription() {
                Id = Guid.NewGuid(),
                PlanId = plainId,
                Status = status,
                BillingReference = billingReference
            };
        }
    }
}