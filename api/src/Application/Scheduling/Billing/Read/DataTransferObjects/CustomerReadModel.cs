using System.Collections.Generic;

namespace DetailingArsenal.Application.Scheduling.Billing {
    public class CustomerReadModel : IDataTransferObject {
        public SubscriptionReadModel? Subscription { get; }
        public List<PaymentMethodReadModel> PaymentMethods { get; }

        public CustomerReadModel(SubscriptionReadModel? subscription = null, List<PaymentMethodReadModel>? paymentMethods = null) {
            Subscription = subscription;
            PaymentMethods = paymentMethods ?? new List<PaymentMethodReadModel>();
        }
    }
}