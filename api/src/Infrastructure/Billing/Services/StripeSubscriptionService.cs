using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Infrastructure {
    public class StripeSubscriptionService : ISubscriptionService {
        private ISubscriptionConfig config;
        private Stripe.SubscriptionService service;
        private ISubscriptionPlanRepo planRepo;

        public StripeSubscriptionService(ISubscriptionConfig config, ISubscriptionPlanRepo planRepo) {
            this.config = config;
            this.planRepo = planRepo;
            service = new Stripe.SubscriptionService();

        }

        public async Task<Subscription> CreateTrialSubscription(Customer customer) {
            var plan = (await planRepo.FindByExternalId(config.DefaultPlan))!;

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
                PlanId = plan.Id
            };

            return sub;
        }
    }
}