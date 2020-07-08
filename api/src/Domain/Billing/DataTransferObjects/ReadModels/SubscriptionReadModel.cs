namespace DetailingArsenal.Domain.Billing {
    public class SubscriptionReadModel : IDataTransferObject {
        public string Name { get; }
        public string Status { get; }
        public SubscriptionPlanPriceReadModel Price { get; }

        public SubscriptionReadModel(string name, string status, SubscriptionPlanPriceReadModel price) {
            Name = name;
            Status = status;
            Price = price;
        }
    }
}