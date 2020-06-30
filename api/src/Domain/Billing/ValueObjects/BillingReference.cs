namespace DetailingArsenal.Domain.Billing {
    public class BillingReference : ValueObject<BillingReference> {
        public string BillingId { get; }
        public BillingReferenceType Type { get; }

        public BillingReference(string billingId, BillingReferenceType type) {
            BillingId = billingId;
            Type = type;
        }
    }
}