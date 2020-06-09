using System.Collections.Generic;

public class SubscriptionPlanInfo : ValueObject<SubscriptionPlanInfo> {
    public string Name { get; }
    public string ExternalId { get; }
    public List<SubscriptionPlanPriceInfo> Prices { get; }

    public SubscriptionPlanInfo(string name, string externalId, List<SubscriptionPlanPriceInfo> prices) {
        Name = name;
        Prices = prices;
        ExternalId = externalId;
    }
}