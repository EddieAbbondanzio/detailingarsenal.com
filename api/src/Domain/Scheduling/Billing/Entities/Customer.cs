using System;
using System.Collections.Generic;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Billing {
    public class Customer : Aggregate<Customer>, IUserEntity, IBillingEntity {
        public Guid UserId { get; }
        public BillingReference BillingReference { get; }
        public Subscription? Subscription { get; set; }
        public List<PaymentMethod> PaymentMethods { get; set; }

        public Customer(Guid id, Guid userId, BillingReference billingReference, Subscription? subscription = null, List<PaymentMethod>? paymentMethods = null) {
            Id = id;
            UserId = userId;
            BillingReference = billingReference;
            Subscription = subscription;
            PaymentMethods = paymentMethods ?? new List<PaymentMethod>();
        }
    }
}