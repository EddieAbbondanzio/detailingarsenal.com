using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Billing;
using Stripe;

namespace DetailingArsenal.Infrastructure.Billing {
    public class CustomerSubscriptionInvoiceUpdatedConverter : StripeWebhookConverter {
        protected override string WebhookType => Events.InvoiceUpdated;

        public override Task<IDomainEvent> Convert(Event e) {
            var subscription = (Stripe.Subscription)e.Data.Object;

            return Task.FromResult(
                new CustomerSubscriptionInvoiceUpdated(
                    Domain.Billing.Subscription.ParseStatus(subscription.Status),
                    Guid.Parse(subscription.Metadata["PlanId"]),
                    subscription.CustomerId
                ) as IDomainEvent
            );
        }
    }
}