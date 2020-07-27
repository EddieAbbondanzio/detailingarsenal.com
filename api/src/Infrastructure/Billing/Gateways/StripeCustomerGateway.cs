using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;
using Stripe;

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

        public async Task<Domain.Billing.Customer> CreateTrialCustomer(User user, SubscriptionPlan trialPlan) {
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

            return Domain.Billing.Customer.Create(
                user.Id,
                new BillingReference(customer.Id, BillingReferenceType.Customer),
                Domain.Billing.Subscription.Create(
                    trialPlan.Id,
                    price.BillingReference.BillingId,
                    subscription.Status,
                    subscription.TrialStart ?? throw new NullReferenceException(),
                    subscription.TrialEnd ?? throw new NullReferenceException(),
                    new BillingReference(
                        subscription.Id,
                        BillingReferenceType.Product
                    ),
                    subscription.CurrentPeriodEnd
                )
            );
        }

        public async Task Delete(Domain.Billing.Customer customer) {
            await customerService.DeleteAsync(customer.BillingReference.BillingId);
        }

        public async Task<Domain.Billing.Customer> GetByBillingId(string billingId) {
            var sCustomer = await customerService.GetAsync(billingId);

            var c = Domain.Billing.Customer.Create(
                Guid.Parse(sCustomer.Metadata["UserId"]),
                new BillingReference(sCustomer.Id, BillingReferenceType.Customer)

            );

            if (sCustomer.Subscriptions.Data.Count > 0) {
                var sSub = sCustomer.Subscriptions.Data[0];

                c.Subscription = Domain.Billing.Subscription.Create(
                    Guid.Parse(sSub.Metadata["Id"]),
                    sSub.Metadata["PriceBillingId"],
                    sSub.Status,
                    sSub.TrialStart ?? throw new NullReferenceException(),
                    sSub.TrialEnd ?? throw new NullReferenceException(),
                    new BillingReference(
                        sSub.Id,
                        BillingReferenceType.Product
                    ),
                    sSub.CurrentPeriodEnd
                );

                c.Subscription.CancellingAtPeriodEnd = sCustomer.Subscriptions.Data[0].CancelAtPeriodEnd;
            }

            /*
            * Payment sources on customer from CustomerService.GetAsync() will always be empty.
            * Need to get them manually instead.
            */
            var sources = await paymentMethodService.ListAsync(new Stripe.PaymentMethodListOptions() {
                Customer = sCustomer.Id,
                Type = "card"
            });


            if (sources.Data.Count > 0) {
                var sCard = sources.Data[0].Card;

                c.PaymentMethod = new Domain.Billing.PaymentMethod(
                    sCard.Brand,
                    sCard.Last4
                );
            }

            return c;
        }


        public async Task CancelSubscriptionAtPeriodEnd(Domain.Billing.Customer customer) {
            if (customer.Subscription == null) {
                throw new InvalidOperationException("No subscription to cancel.");
            }

            var opts = new SubscriptionUpdateOptions {
                CancelAtPeriodEnd = false
            };

            await subscriptionService.UpdateAsync(customer.Subscription.BillingReference.BillingId, opts);
            customer.Subscription.CancellingAtPeriodEnd = true;
        }

        public async Task UndoCancellingSubscription(Domain.Billing.Customer customer) {
            if (customer.Subscription == null) {
                throw new InvalidOperationException("No subscription to cancel.");
            }

            if (!customer.Subscription.CancellingAtPeriodEnd) {
                throw new InvalidOperationException("Cannot undo what has not been done");
            }

            var opts = new SubscriptionUpdateOptions {
                CancelAtPeriodEnd = false
            };

            await subscriptionService.UpdateAsync(customer.Subscription.BillingReference.BillingId, opts);
            customer.Subscription.CancellingAtPeriodEnd = false;
        }
    }
}