using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Infrastructure.Billing {
    public class StripeCustomerGateway : ICustomerGateway {
        Stripe.CustomerService customerService;
        Stripe.SubscriptionService subscriptionService;
        Stripe.PaymentMethodService paymentMethodService;
        ISubscriptionConfig config;

        public StripeCustomerGateway(ISubscriptionConfig config) {
            customerService = new Stripe.CustomerService();
            subscriptionService = new Stripe.SubscriptionService();
            paymentMethodService = new Stripe.PaymentMethodService();
            this.config = config;
        }

        public async Task<Customer> CreateTrialCustomer(User user, SubscriptionPlan trialPlan) {
            var custCreateOpts = new Stripe.CustomerCreateOptions {
                Email = user.Email,
                Metadata = new Dictionary<string, string>()
            };

            custCreateOpts.Metadata["UserId"] = user.Id.ToString();

            var customer = await customerService.CreateAsync(custCreateOpts);

            var price = trialPlan.Prices.Find(p => p.BillingReference.BillingId == config.DefaultPrice)!;

            var subCreateOpts = new Stripe.SubscriptionCreateOptions {
                Customer = customer.Id,
                Items = new List<Stripe.SubscriptionItemOptions> {
                    new Stripe.SubscriptionItemOptions {
                        Price = price.BillingReference.BillingId
                    }
                },
                TrialPeriodDays = config.TrialPeriod,
                Metadata = new Dictionary<string, string>()
            };

            subCreateOpts.Metadata = new Dictionary<string, string>();
            subCreateOpts.Metadata["Id"] = trialPlan.Id.ToString();
            subCreateOpts.Metadata["PriceBillingId"] = price.BillingReference.BillingId;

            var subscription = await subscriptionService.CreateAsync(subCreateOpts);

            return Customer.Create(
                user.Id,
                new BillingReference(customer.Id, BillingReferenceType.Customer),
                Subscription.Create(
                    trialPlan.Id,
                    price.BillingReference.BillingId,
                    subscription.Status,
                    subscription.TrialStart ?? throw new NullReferenceException(),
                    subscription.TrialEnd ?? throw new NullReferenceException(),
                    new BillingReference(
                        subscription.Id,
                        BillingReferenceType.Product
                    )
                )
            );
        }

        public async Task Delete(Customer customer) {
            await customerService.DeleteAsync(customer.BillingReference.BillingId);
        }

        public async Task<Customer> GetByBillingId(string billingId) {
            var sCustomer = await customerService.GetAsync(billingId);

            // Safe to assume the customer will always have 1!.
            var sSub = sCustomer.Subscriptions.Data[0];

            var c = Customer.Create(
                Guid.Parse(sCustomer.Metadata["UserId"]),
                new BillingReference(sCustomer.Id, BillingReferenceType.Customer),
                Subscription.Create(
                    Guid.Parse(sSub.Metadata["Id"]),
                    sSub.Metadata["PriceBillingId"],
                    sSub.Status,
                    sSub.TrialStart ?? throw new NullReferenceException(),
                    sSub.TrialEnd ?? throw new NullReferenceException(),
                    new BillingReference(
                        sSub.Id,
                        BillingReferenceType.Product
                    )
                )
            );

            var sources = await paymentMethodService.ListAsync(new Stripe.PaymentMethodListOptions() {
                Customer = sCustomer.Id,
                Type = "card"
            });


            if (sources.Data.Count > 0) {
                var sCard = sources.Data[0].Card;

                if (sCard == null) {
                    throw new InvalidOperationException("Hey dummy. Don't hardcode this if you want to support other payment methods.");
                }

                c.PaymentMethod = new PaymentMethod(
                    sCard.Brand,
                    sCard.Last4
                );
            }

            return c;
        }
    }
}