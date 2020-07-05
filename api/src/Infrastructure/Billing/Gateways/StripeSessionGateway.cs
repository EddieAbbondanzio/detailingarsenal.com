using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Billing;
using Stripe.Checkout;

namespace DetailingArsenal.Infrastructure.Billing {
    public class StripeSessionGateway : ISessionGateway {
        ISubscriptionConfig config;
        SessionService sessionService;

        public StripeSessionGateway(ISubscriptionConfig config) {
            this.config = config;
            this.sessionService = new SessionService();
        }

        public async Task<BillingReference> CreateSession(Customer customer, string priceBillingId) {
            var opts = new SessionCreateOptions {
                PaymentMethodTypes = new List<string> {
                    "card"
                },
                LineItems = new List<SessionLineItemOptions>() {
                    new SessionLineItemOptions {
                        Price = priceBillingId,
                        Quantity = 1
                    }
                },
                Mode = "subscription",
                SuccessUrl = config.SuccessUrl,
                CancelUrl = config.CancelUrl
            };

            var session = await sessionService.CreateAsync(opts);
            return new BillingReference(session.Id, BillingReferenceType.Session);
        }
    }
}