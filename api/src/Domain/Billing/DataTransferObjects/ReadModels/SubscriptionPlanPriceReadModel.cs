namespace DetailingArsenal.Domain.Billing {
    public class SubscriptionPlanPriceReadModel : IDataTransferObject {
        public decimal Price { get; }
        public string Interval { get; }
        public string BillingId { get; }

        public SubscriptionPlanPriceReadModel(decimal price, string interval, string billingId) {
            Price = price;
            Interval = interval;
            BillingId = billingId;
        }
    }
}