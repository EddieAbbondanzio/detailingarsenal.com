namespace DetailingArsenal.Domain.Billing {
    public class SubscriptionReadModel : IDataTransferObject {
        public string Name { get; }
        public string Status { get; }
        public decimal Price { get; }
        public string PriceInterval { get; }

        public SubscriptionReadModel(string name, string status, decimal price, string priceInterval) {
            Name = name;
            Status = status;
            Price = price;
            PriceInterval = priceInterval;
        }
    }
}