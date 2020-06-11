namespace DetailingArsenal.Domain {
    public class SubscriptionPlanPrice : Entity<SubscriptionPlanPrice> {
        public decimal Price { get; set; }
        public string Interval { get; set; } = null!;
        public string ExternalId { get; set; } = null!;
    }
}