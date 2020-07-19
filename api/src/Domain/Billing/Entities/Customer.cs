using System;

namespace DetailingArsenal.Domain.Billing {
    public class Customer : Aggregate<Customer>, IUserEntity, IBillingEntity {
        public Guid UserId { get; set; }
        public BillingReference BillingReference { get; set; } = null!;
        public Subscription? Subscription { get; set; } = null!;
        public PaymentMethod? PaymentMethod { get; set; }

        public static Customer Create(Guid userId, BillingReference reference, Subscription? subscription = null, PaymentMethod? paymentMethod = null) {
            return new Customer() {
                Id = Guid.NewGuid(),
                UserId = userId,
                BillingReference = reference,
                Subscription = subscription,
                PaymentMethod = paymentMethod
            };
        }
    }
}