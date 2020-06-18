namespace DetailingArsenal.Domain {
    public class ExternalSubscriptionPlanPrice : ValueObject<ExternalSubscriptionPlanPrice> {
        public decimal Price { get; }
        public string Interval { get; }
        public string ExternalId { get; }

        public ExternalSubscriptionPlanPrice(decimal price, string interval, string externalId) {
            Price = price;
            Interval = interval;
            ExternalId = externalId;
        }
    }
}