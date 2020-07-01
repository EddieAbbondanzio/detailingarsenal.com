using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Infrastructure.Billing {
    public class StripeCustomerGateway : ICustomerGateway {
        Stripe.CustomerService customerService;
        Stripe.SubscriptionService subscriptionService;

        public StripeCustomerGateway() {
            customerService = new Stripe.CustomerService();
            subscriptionService = new Stripe.SubscriptionService();
        }

        public async Task<Customer> CreateTrialCustomer(User user, SubscriptionPlan trialPlan) {
            var customer = await customerService.CreateAsync(new Stripe.CustomerCreateOptions {
                Email = user.Email
            });

            var subscription = await subscriptionService.CreateAsync(new Stripe.SubscriptionCreateOptions {
                Customer = customer.Id,
                Items = new List<Stripe.SubscriptionItemOptions> {
                    new Stripe.SubscriptionItemOptions {
                        Price = trialPlan.Prices.Find(p => p.Interval == "year")!.BillingReference.BillingId
                    }
                }
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