namespace DetailingArsenal.Domain.Billing {
    public class CustomerReadModel : IDataTransferObject {
        public SubscriptionReadModel? Subscription { get; }
        public PaymentMethodReadModel? PaymentMethod { get; }

        public CustomerReadModel(SubscriptionReadModel? subscription = null, PaymentMethodReadModel? paymentMethod = null) {
            Subscription = subscription;
            PaymentMethod = paymentMethod;
        }
    }
}