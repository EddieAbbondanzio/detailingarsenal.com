using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Infrastructure {
    public class StripeSubscriptionService : ISubscriptionService {
        private ISubscriptionConfig config;
        private Stripe.SubscriptionService service;

        public StripeSubscriptionService(ISubscriptionConfig config) {
            this.config = config;
            service = new Stripe.SubscriptionService();

        }

        public async Task<Subscription> CreateTrialSubscription(Customer customer) {
            var options = new Stripe.SubscriptionCreateOptions() {
                Customer = customer.Info.ExternalId,
                Items = new System.Collections.Generic.List<Stripe.SubscriptionItemOptions>() {
                new Stripe.SubscriptionItemOptions() {
                    Price = config.DefaultPrice,
                }
            },
                TrialPeriodDays = config.TrialPeriod
            };

            var s = await service.CreateAsync(options);
            return Map(s);
        }

        Subscription Map(Stripe.Subscription subscription) {
            return new Subscription() {
                ExternalId = subscription.Id,
                Status = subscription.Status
            };
        }
    }
}