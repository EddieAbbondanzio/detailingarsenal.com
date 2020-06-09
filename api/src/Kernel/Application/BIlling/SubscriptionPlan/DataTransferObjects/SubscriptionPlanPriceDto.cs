public class SubscriptionPlanPriceDto : IDataTransferObject {
    public decimal Price { get; set; }
    public string Interval { get; set; } = null!;
}