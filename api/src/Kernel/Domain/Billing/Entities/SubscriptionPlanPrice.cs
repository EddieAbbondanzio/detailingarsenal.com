public class SubscriptionPlanPrice : Entity<SubscriptionPlanPrice> {
    public decimal Price { get; set; }
    public string Interval { get; set; } = null!;
}