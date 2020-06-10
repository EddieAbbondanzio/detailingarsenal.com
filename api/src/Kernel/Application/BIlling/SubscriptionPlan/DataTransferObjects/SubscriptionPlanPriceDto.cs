public class SubscriptionPlanPriceDto : IDataTransferObject {
    public decimal Price { get; set; }
    public string Interval { get; set; } = null!;
    public string ExternalId { get; set; } = null!;
}