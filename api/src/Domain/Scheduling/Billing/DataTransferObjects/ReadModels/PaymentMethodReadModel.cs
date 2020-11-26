namespace DetailingArsenal.Domain.Scheduling.Billing {
    public class PaymentMethodReadModel : IDataTransferObject {
        public string Brand { get; }
        public string Last4 { get; }
        public bool IsDefault { get; }
        public string ExpirationMonth { get; }
        public string ExpirationYear { get; }

        public PaymentMethodReadModel(string brand, string last4, bool isDefault, string expirationMonth, string expirationYear) {
            Brand = brand;
            Last4 = last4;
            IsDefault = isDefault;
            ExpirationMonth = expirationMonth;
            ExpirationYear = expirationYear;
        }
    }
}