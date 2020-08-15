using System;

namespace DetailingArsenal.Domain.Billing {
    public class PaymentMethod : Entity<PaymentMethod>, IBillingEntity {
        public string Brand { get; }
        public string Last4 { get; }
        public bool IsDefault { get; }
        public ExpirationDate Expiration { get; }
        public BillingReference BillingReference { get; }

        public PaymentMethod(string brand, string last4, bool isDefault, ExpirationDate expiration, BillingReference billingReference) {
            Id = Guid.NewGuid();
            Brand = brand;
            Last4 = last4;
            IsDefault = isDefault;
            Expiration = expiration;
            BillingReference = billingReference;
        }

        public PaymentMethod(Guid id, string brand, string last4, bool isDefault, ExpirationDate expiration, BillingReference billingReference) {
            Id = id;
            Brand = brand;
            Last4 = last4;
            IsDefault = isDefault;
            Expiration = expiration;
            BillingReference = billingReference;
        }
    }
}