using System.Collections.Generic;

public class SubscriptionPlanDto : IDataTransferObject {
    public string Name { get; set; } = null!;
    public List<SubscriptionPlanPriceDto> Prices { get; set; } = null!;
    public string ExternalId { get; set; } = null!;
}