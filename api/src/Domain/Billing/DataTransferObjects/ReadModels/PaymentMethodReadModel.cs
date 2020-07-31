namespace DetailingArsenal.Domain.Billing {
    public class PaymentMethodReadModel : IDataTransferObject {
        public string Brand { get; }
        public string Last4 { get; }
        public bool IsDefault { get; }

        public PaymentMethodReadModel(string brand, string last4, bool isDefault) {
            Brand = brand;
            Last4 = last4;
            IsDefault = isDefault;
        }
    }
}