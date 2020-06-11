namespace DetailingArsenal.Domain {
    public class SubscriptionPlanPriceInfo : ValueObject<SubscriptionPlanPriceInfo> {
        public decimal Price { get; }
        public string Interval { get; }
        public string ExternalId { get; }

        public SubscriptionPlanPriceInfo(decimal price, string interval, string externalId) {
            Price = price;
            Interval = interval;
            ExternalId = externalId;
        }
    }
}