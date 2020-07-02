using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Infrastructure.Billing {
    public class StripeCustomerGateway : ICustomerGateway {
        Stripe.CustomerService customerService;
        Stripe.SubscriptionService subscriptionService;
        ISubscriptionConfig config;

        public StripeCustomerGateway(ISubscriptionConfig config) {
            customerService = new Stripe.CustomerService();
            subscriptionService = new Stripe.SubscriptionService();
            this.config = config;
        }

        public async Task<Customer> CreateTrialCustomer(User user, SubscriptionPlan trialPlan) {
            var customer = await customerService.CreateAsync(new Stripe.CustomerCreateOptions {
                Email = user.Email
            });

            var subscription = await subscriptionService.CreateAsync(new Stripe.SubscriptionCreateOptions {
                Customer = customer.Id,
                Items = new List<Stripe.SubscriptionItemOptions> {
                    new Stripe.SubscriptionItemOptions {
                        Price = trialPlan.Prices.Find(p => p.BillingReference.BillingId == config.DefaultPrice)!.BillingReference.BillingId
                    }
                },
                TrialPeriodDays = config.TrialPeriod
            });

            return Customer.Create(
                user.Id,
                new BillingReference(customer.Id, BillingReferenceType.Customer),
                Subscription.Create(
                    trialPlan.Id,
                    subscription.Status,
                    new BillingReference(
                        subscription.Id,
                        BillingReferenceType.Product
                    )
                )
            );
        }
    }
}