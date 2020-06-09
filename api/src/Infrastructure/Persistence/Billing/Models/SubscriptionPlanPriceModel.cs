using System;

public class SubscriptionPlanPriceModel {
    public Guid Id { get; set; }
    public Guid PlanId { get; set; }
    public decimal Price { get; set; }
    public string Interval { get; set; } = null!;
}