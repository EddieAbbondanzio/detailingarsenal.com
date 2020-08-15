using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Billing;
using Serilog;
using Stripe;

namespace DetailingArsenal.Infrastructure.Billing {
    public class StripeWebhookParser : IBillingWebhookParser {
        IBillingConfig config;
        IEnumerable<StripeWebhookConverter> converters;

        public StripeWebhookParser(IEnumerable<StripeWebhookConverter> converters, IBillingConfig config) {
            this.config = config;
            this.converters = converters;
        }

        public async Task<TDomainEvent?> Parse<TDomainEvent>(Stream stream, string signature) where TDomainEvent : class, IDomainEvent {
            string json = await new StreamReader(stream).ReadToEndAsync();

            try {
                var stripeEvent = EventUtility.ConstructEvent(json, signature, config.WebhookSecret);

                var converter = converters.First(p => p.CanConvert(stripeEvent.Type));
                var domainEvent = await converter.Convert(stripeEvent);

                // Gross cast.
                return domainEvent as TDomainEvent;
            } catch (StripeException e) {
                Log.Error("Failed to parse Stripe webhook", e);
                return null;
            }
        }
    }
}