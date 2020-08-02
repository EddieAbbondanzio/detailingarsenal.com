using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Billing;
using Stripe;

namespace DetailingArsenal.Infrastructure.Billing {
    public class StripeBillingEventService : IBillingEventService {
        IEnumerable<IBillingEventParser> parsers;
        ISubscriptionConfig config;

        public StripeBillingEventService(IEnumerable<IBillingEventParser> parsers, ISubscriptionConfig config) {
            this.parsers = parsers;
            this.config = config;
        }

        public async Task<TDomainEvent> Parse<TDomainEvent>(Stream stream, string signature) where TDomainEvent : IDomainEvent {
            string json = await new StreamReader(stream).ReadToEndAsync();

            try {
                var stripeEvent = EventUtility.ConstructEvent(json,)
            }

            throw new System.NotImplementedException();
        }
    }
}