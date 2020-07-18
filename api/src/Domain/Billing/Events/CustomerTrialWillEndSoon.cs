namespace DetailingArsenal.Domain.Billing {
    public class CustomerTrialWillEndSoon : IDomainEvent {
        public string CustomerBillingId { get; }

        public CustomerTrialWillEndSoon(string customerBillingId) {
            CustomerBillingId = customerBillingId;
        }
    }
}