namespace DetailingArsenal.Domain.Billing {
    public class CustomerSubscriptionInvoiceUpdated : IDomainEvent {
        public string SubscriptionStatus { get; }
        public string CustomerBillingId { get; }

        public CustomerSubscriptionInvoiceUpdated(string subscriptionStatus, string customerBillingId) {
            SubscriptionStatus = subscriptionStatus;
            CustomerBillingId = customerBillingId;
        }
    }
}