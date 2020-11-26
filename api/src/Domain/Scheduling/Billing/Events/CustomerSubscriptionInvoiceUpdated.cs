using System;

namespace DetailingArsenal.Domain.Scheduling.Billing {
    public class CustomerSubscriptionInvoiceUpdated : IDomainEvent {
        public SubscriptionStatus SubscriptionStatus { get; }
        public Guid PlanId { get; }
        public string CustomerBillingId { get; }

        public CustomerSubscriptionInvoiceUpdated(SubscriptionStatus subscriptionStatus, Guid planId, string customerBillingId) {
            SubscriptionStatus = subscriptionStatus;
            PlanId = planId;
            CustomerBillingId = customerBillingId;
        }
    }
}