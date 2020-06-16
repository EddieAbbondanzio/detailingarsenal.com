using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Core {
    /// <summary>
    /// Publisher to notify subscribers when a domain event they
    /// care about has occured. Best used to initiate parallel tasks.
    /// </summary>
    public sealed class DomainEvents {
        private IDomainEventSubscriberCollection subscriberCollection;

        public DomainEvents(IDomainEventSubscriberCollection subscriberCollection) {
            this.subscriberCollection = subscriberCollection;
        }

#pragma warning disable 1998, 4014
        public async Task Dispatch<TEvent>(TEvent domainEvent) where TEvent : class, IDomainEvent {
            IEnumerable<IDomainEventSubscriber<TEvent>> subscribers = subscriberCollection.GetSubscibers<TEvent>();

            foreach (IDomainEventSubscriber<TEvent> subscriber in subscribers) {
                // We don't want to wait for it to finish
                subscriber.Notify(domainEvent);
            }
        }
    }
#pragma warning restore 1998, 4014
}