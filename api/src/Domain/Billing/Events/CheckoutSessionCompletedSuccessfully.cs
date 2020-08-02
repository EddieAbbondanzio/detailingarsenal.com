namespace DetailingArsenal.Domain.Billing {
    public class CheckoutSessionCompletedSuccessfully : IDomainEvent {
        public string CustomerBillingId { get; }
        public string NewPaymentMethodBillingId { get; }

        public CheckoutSessionCompletedSuccessfully(string customerBillingId, string newPaymentMethodBillingId) {
            CustomerBillingId = customerBillingId;
            NewPaymentMethodBillingId = newPaymentMethodBillingId;
        }
    }
}