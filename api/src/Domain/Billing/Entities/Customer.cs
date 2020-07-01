using System;

namespace DetailingArsenal.Domain.Billing {
    public class Customer : Aggregate<Customer>, IUserEntity, IBillingEntity {
        public Guid UserId { get; set; }
        public BillingReference BillingReference { get; set; } = null!;
        public Subscription Subscription { get; set; } = null!;

        public static Customer Create(Guid userId, BillingReference reference, Subscription subscription) {
            return new Customer() {
                Id = Guid.NewGuid(),
                UserId = userId,
                BillingReference = reference,
                Subscription = subscription
            };
        }
    }
}