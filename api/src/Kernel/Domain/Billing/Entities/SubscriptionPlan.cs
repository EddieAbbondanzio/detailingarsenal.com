using System.Collections.Generic;

public class SubscriptionPlan : Entity<SubscriptionPlan> {
    public string Name { get; set; } = null!;
    public string ExternalId { get; set; } = null!;
    public List<SubscriptionPlanPrice> Prices { get; set; } = new List<SubscriptionPlanPrice>();
}