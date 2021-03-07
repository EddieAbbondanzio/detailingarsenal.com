using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;
using Stripe;

namespace DetailingArsenal.Infrastructure.Scheduling.Billing {
    [DependencyInjection(RegisterAs = typeof(ICustomerGateway))]
    public class StripeCustomerGateway : ICustomerGateway {
        Stripe.CustomerService customerService;
        Stripe.SubscriptionService subscriptionService;
        Stripe.PaymentMethodService paymentMethodService;
        IBillingConfig config;

        public StripeCustomerGateway(IBillingConfig config) {
            customerService = new Stripe.CustomerService();
            subscriptionService = new Stripe.SubscriptionService();
            paymentMethodService = new Stripe.PaymentMethodService();
            this.config = config;
        }

        public async Task<Domain.Scheduling.Billing.Customer> CreateTrialCustomer(User user, SubscriptionPlan trialPlan) {
            var customerId = Guid.NewGuid();

            var custCreateOpts = new Stripe.CustomerCreateOptions {
                Email = user.Email,
                Metadata = new Dictionary<string, string>(new[] {
                    KeyValuePair.Create("Id", customerId.ToString()),
                    KeyValuePair.Create("UserId", user.Id.ToString()),
                })
            };

            var customer = await customerService.CreateAsync(custCreateOpts);

            var price = trialPlan.Prices.Find(p => p.BillingReference.BillingId == config.DefaultPrice)!;
            var subId = Guid.NewGuid();

            var subCreateOpts = new Stripe.SubscriptionCreateOptions {
                Customer = customer.Id,
                Items = new List<Stripe.SubscriptionItemOptions> {
                    new Stripe.SubscriptionItemOptions {
                        Price = price.BillingReference.BillingId
                    }
                },
                TrialPeriodDays = config.TrialPeriod,
                Metadata = new Dictionary<string, string>(new[] {
                    KeyValuePair.Create("Id", subId.ToString()),
                    KeyValuePair.Create("PlanId", trialPlan.Id.ToString()),
                    KeyValuePair.Create("PriceBillingId", price.BillingReference.BillingId)
                })
            };

            var subscription = await subscriptionService.CreateAsync(subCreateOpts);

            return new Domain.Scheduling.Billing.Customer(
                customerId,
                user.Id,
                BillingReference.Customer(customer.Id),
                new Domain.Scheduling.Billing.Subscription(
                    subId,
                    Domain.Scheduling.Billing.Subscription.ParseStatus(subscription.Status),
                    new Domain.Scheduling.Billing.Period(
                        subscription.TrialStart ?? throw new NullReferenceException(),
                        subscription.TrialEnd ?? throw new NullReferenceException()
                    ),
                    new Domain.Scheduling.Billing.Period(
                        subscription.CurrentPeriodStart,
                        subscription.CurrentPeriodEnd
                    ),
                    subscription.CancelAtPeriodEnd,
                    new SubscriptionPlanReference(
                        trialPlan.Id,
                        price.BillingReference.BillingId
                    ),
                    BillingReference.Subscription(subscription.Id)
                )
            );
        }

        public async Task Delete(Domain.Scheduling.Billing.Customer customer) {
            await customerService.DeleteAsync(customer.BillingReference.BillingId);
        }

        public async Task<Domain.Scheduling.Billing.Customer> GetByBillingId(string billingId) {
            var stripeCustomer = await customerService.GetAsync(billingId);

            var customer = new Domain.Scheduling.Billing.Customer(
                Guid.Parse(stripeCustomer.Metadata["Id"]),
                Guid.Parse(stripeCustomer.Metadata["UserId"]),
                BillingReference.Customer(stripeCustomer.Id)
            );

            if (stripeCustomer.Subscriptions.Data.Count > 0) {
                var stripeSubscription = stripeCustomer.Subscriptions.Data[0];

                customer.Subscription = new Domain.Scheduling.Billing.Subscription(
                    Guid.Parse(stripeSubscription.Metadata["Id"]),
                    Domain.Scheduling.Billing.Subscription.ParseStatus(stripeSubscription.Status),
                    new Domain.Scheduling.Billing.Period(
                    stripeSubscription.TrialStart ?? throw new NullReferenceException(),
                    stripeSubscription.TrialEnd ?? throw new NullReferenceException()
                    ),
                    new Domain.Scheduling.Billing.Period(
                        stripeSubscription.CurrentPeriodStart,
                        stripeSubscription.CurrentPeriodEnd
                    ),
                    stripeSubscription.CancelAtPeriodEnd,
                    new SubscriptionPlanReference(
                        Guid.Parse(stripeSubscription.Metadata["PlanId"]),
                        stripeSubscription.Metadata["PriceBillingId"]
                    ),
                    BillingReference.Subscription(stripeSubscription.Id)
                );
            }

            /*
            * Payment sources on customer from CustomerService.GetAsync() will always be empty.
            * Need to get them manually instead.
            */
            var paymentMethods = await paymentMethodService.ListAsync(new Stripe.PaymentMethodListOptions() {
                Customer = stripeCustomer.Id,
                Type = "card"
            });

            for (int i = 0; i < paymentMethods.Data.Count; i++) {
                var paymentMethod = paymentMethods.Data[i];

                // Add our id to the card if this is the first time we've seen it.
                if (!paymentMethod.Metadata.ContainsKey("Id")) {
                    paymentMethod = await paymentMethodService.UpdateAsync(paymentMethod.Id, new PaymentMethodUpdateOptions() {
                        Metadata = new Dictionary<string, string>(new[] {
                            KeyValuePair.Create("Id", Guid.NewGuid().ToString())
                        })
                    });
                }

                var card = paymentMethod.Card;

                customer.PaymentMethods.Add(new Domain.Scheduling.Billing.PaymentMethod(
                    Guid.Parse(paymentMethod.Metadata["Id"]),
                    card.Brand,
                    card.Last4,
                    // Stripe won't set default unless there is more than 1 payment method on a customer
                    paymentMethods.Data.Count == 1 ? true : stripeCustomer.InvoiceSettings.DefaultPaymentMethodId == paymentMethod.Id,
                    new ExpirationDate(card.ExpMonth.ToString(), card.ExpYear.ToString()),
                    BillingReference.PaymentMethod(paymentMethod.Id)
                ));
            }

            return customer;
        }


        public async Task CancelSubscriptionAtPeriodEnd(Domain.Scheduling.Billing.Customer customer) {
            if (customer.Subscription == null) {
                throw new InvalidOperationException("No subscription to cancel.");
            }

            var opts = new SubscriptionUpdateOptions {
                CancelAtPeriodEnd = true
            };

            await subscriptionService.UpdateAsync(customer.Subscription.BillingReference.BillingId, opts);
            customer.Subscription.CancellingAtPeriodEnd = true;
        }

        public async Task UndoCancellingSubscription(Domain.Scheduling.Billing.Customer customer) {
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