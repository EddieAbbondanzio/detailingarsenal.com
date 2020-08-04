using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Billing;
using Stripe.Checkout;

namespace DetailingArsenal.Infrastructure.Billing {
    public class StripeCheckoutSessionGateway : ICheckoutSessionGateway {
        IBillingConfig config;
        SessionService sessionService;

        public StripeCheckoutSessionGateway(IBillingConfig config) {
            this.config = config;
            this.sessionService = new SessionService();
        }

        public async Task<BillingReference> CreateSession(Customer customer, string priceBillingId) {
            var opts = new SessionCreateOptions {
                PaymentMethodTypes = new List<string> {
                    "card"
                },
                Customer = customer.BillingReference.BillingId,
                SetupIntentData = new SessionSetupIntentDataOptions() {
                    Metadata = new Dictionary<string, string> {
                        { "customer_id", customer.BillingReference.BillingId },
                    }
                },
                Mode = "setup",
                SuccessUrl = config.SuccessUrl,
                CancelUrl = config.CancelUrl
            };

            if (customer.Subscription != null) {
                opts.SetupIntentData.Metadata.Add("subscription_id", customer.Subscription.BillingReference.BillingId);
            }

            var session = await sessionService.CreateAsync(opts);
            return BillingReference.CheckoutSession(session.Id);
        }
    }
}