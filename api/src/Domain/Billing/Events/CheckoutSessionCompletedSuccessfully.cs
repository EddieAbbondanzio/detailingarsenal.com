namespace DetailingArsenal.Domain.Billing {
    public class CheckoutSessionCompletedSuccessfully : IDomainEvent {
        public string CustomerBillingId { get; }

        public CheckoutSessionCompletedSuccessfully(string customerBillingId) {
            CustomerBillingId = customerBillingId;
        }
    }
}