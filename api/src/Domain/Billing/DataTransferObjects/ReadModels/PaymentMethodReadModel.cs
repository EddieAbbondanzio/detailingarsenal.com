namespace DetailingArsenal.Domain.Billing {
    public class PaymentMethodReadModel : IDataTransferObject {
        public string Brand { get; }
        public string Last4 { get; }

        public PaymentMethodReadModel(string brand, string last4) {
            Brand = brand;
            Last4 = last4;
        }
    }
}