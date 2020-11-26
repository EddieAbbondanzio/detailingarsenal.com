using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;
using Stripe;
using Stripe.Checkout;

namespace DetailingArsenal.Infrastructure.Scheduling.Billing {
    /// <summary>
    /// Converter that retrieves additional information on a Stripe checkout session completed
    /// successfully event before returning our domain event.
    /// </summary>
    public class CheckoutSessionCompletedConverter : StripeWebhookConverter {
        protected override string WebhookType => Events.CheckoutSessionCompleted;

        public async override Task<IDomainEvent> Convert(Event e) {
            var session = (Session)e.Data.Object;

            var siService = new Stripe.SetupIntentService();
            var setupIntent = await siService.GetAsync(session.SetupIntentId);

            return new CheckoutSessionCompletedSuccessfully(
                session.CustomerId,
                setupIntent.PaymentMethodId
            );
        }
    }
}