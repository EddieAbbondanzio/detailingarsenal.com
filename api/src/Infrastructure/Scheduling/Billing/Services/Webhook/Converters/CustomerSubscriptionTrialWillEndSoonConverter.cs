using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Billing;
using Stripe;

namespace DetailingArsenal.Infrastructure.Billing {
    public class CustomerSubscriptionTrialWillEndSoonConverter : StripeWebhookConverter {
        protected override string WebhookType => Events.CustomerSubscriptionTrialWillEnd;

        public override Task<IDomainEvent> Convert(Event e) {
            var subscription = (Stripe.Subscription)e.Data.Object;

            return Task.FromResult(new CustomerTrialWillEndSoon(
                subscription.CustomerId
            ) as IDomainEvent);
        }
    }
}