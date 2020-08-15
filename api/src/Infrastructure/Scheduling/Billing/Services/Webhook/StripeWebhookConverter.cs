using System.Threading.Tasks;
using DetailingArsenal.Domain;
using Stripe;

namespace DetailingArsenal.Infrastructure.Billing {
    public abstract class StripeWebhookConverter {
        /// <summary>
        /// The Stripe event type it supports converting.
        /// </summary>
        protected abstract string WebhookType { get; }

        /// <summary>
        /// Check to see if the convert supports a specific stripe event type.
        /// </summary>
        /// <param name="eventType">The event type identifier to check.</param>
        /// <returns>True if converting is supported.</returns>
        public bool CanConvert(string eventType) {
            return WebhookType == eventType;
        }

        public abstract Task<IDomainEvent> Convert(Event e);
    }
}