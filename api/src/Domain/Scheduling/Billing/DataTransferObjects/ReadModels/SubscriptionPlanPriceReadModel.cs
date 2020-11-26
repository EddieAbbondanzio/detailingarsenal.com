namespace DetailingArsenal.Domain.Scheduling.Billing {
    public class SubscriptionPlanPriceReadModel : IDataTransferObject {
        public decimal Amount { get; }
        public string Interval { get; }
        public string BillingId { get; }

        public SubscriptionPlanPriceReadModel(decimal amount, string interval, string billingId) {
            Amount = amount;
            Interval = interval;
            BillingId = billingId;
        }
    }
}