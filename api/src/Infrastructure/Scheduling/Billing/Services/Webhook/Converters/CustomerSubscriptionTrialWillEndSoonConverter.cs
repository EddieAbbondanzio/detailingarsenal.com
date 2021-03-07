using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;
using Stripe;

namespace DetailingArsenal.Infrastructure.Scheduling.Billing {
    [DependencyInjection(RegisterAs = typeof(StripeWebhookConverter))]
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