namespace DetailingArsenal.Domain.Scheduling.Billing {
    public class BillingReference : ValueObject<BillingReference> {
        public string BillingId { get; }
        public BillingReferenceType Type { get; }

        public BillingReference(string billingId, BillingReferenceType type) {
            BillingId = billingId;
            Type = type;
        }

        public static BillingReference Product(string billingId) => new BillingReference(billingId, BillingReferenceType.Product);
        public static BillingReference Price(string billingId) => new BillingReference(billingId, BillingReferenceType.Price);
        public static BillingReference Customer(string billingId) => new BillingReference(billingId, BillingReferenceType.Customer);
        public static BillingReference Subscription(string billingId) => new BillingReference(billingId, BillingReferenceType.Subscription);
        public static BillingReference CheckoutSession(string billingId) => new BillingReference(billingId, BillingReferenceType.CheckoutSession);
        public static BillingReference PaymentMethod(string billingId) => new BillingReference(billingId, BillingReferenceType.PaymentMethod);
    }
}