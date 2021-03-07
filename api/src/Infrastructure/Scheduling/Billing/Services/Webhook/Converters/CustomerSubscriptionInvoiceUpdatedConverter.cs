using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;
using Stripe;

namespace DetailingArsenal.Infrastructure.Scheduling.Billing {
    [DependencyInjection(RegisterAs = typeof(StripeWebhookConverter))]
    public class CustomerSubscriptionInvoiceUpdatedConverter : StripeWebhookConverter {
        protected override string WebhookType => Events.InvoiceUpdated;

        public override Task<IDomainEvent> Convert(Event e) {
            var subscription = (Stripe.Subscription)e.Data.Object;

            return Task.FromResult(
                new CustomerSubscriptionInvoiceUpdated(
                    Domain.Scheduling.Billing.Subscription.ParseStatus(subscription.Status),
                    Guid.Parse(subscription.Metadata["PlanId"]),
                    subscription.CustomerId
                ) as IDomainEvent
            );
        }
    }
}