namespace DetailingArsenal.Domain.Billing {
    public class PaymentMethod : ValueObject<PaymentMethod> {
        public string Brand { get; }
        public string Last4 { get; }

        public PaymentMethod(string brand, string last4) {
            Brand = brand;
            Last4 = last4;
        }
    }
}