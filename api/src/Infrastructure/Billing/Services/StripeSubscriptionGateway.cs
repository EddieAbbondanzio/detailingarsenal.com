using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Infrastructure {
    public class StripeSubscriptionGateway : ISubscriptionGateway {
        private ISubscriptionConfig config;
        private Stripe.SubscriptionService service;
        private ISubscriptionPlanRepo planRepo;

        public StripeSubscriptionGateway(ISubscriptionConfig config, ISubscriptionPlanRepo planRepo) {
            this.config = config;
            this.planRepo = planRepo;
            service = new Stripe.SubscriptionService();

        }

        public async Task<Subscription> CreateTrialSubscription(SubscriptionPlan plan, Customer customer) {
            var options = new Stripe.SubscriptionCreateOptions() {
                Customer = customer.Info.ExternalId,
                Items = new System.Collections.Generic.List<Stripe.SubscriptionItemOptions>() {
                new Stripe.SubscriptionItemOptions() {
                    // Price is all Stripe needs to find product
                    Price = config.DefaultPrice,
                }
            },
                TrialPeriodDays = config.TrialPeriod
            };

            var s = await service.CreateAsync(options);

            var sub = new Subscription() {
                Id = Guid.NewGuid(),
                UserId = customer.UserId,
                ExternalId = s.Id,
                PlanId = plan.Id,
                Status = s.Status
            };

            return sub;
        }
    }
}