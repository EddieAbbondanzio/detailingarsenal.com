using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace DetailingArsenal.Infrastructure {
    [DependencyInjection(RegisterAs = typeof(IDomainEventSubscriberCollection))]
    public sealed class DomainEventSubscriberCollection : IDomainEventSubscriberCollection {
        private IServiceProvider serviceProvider;

        public DomainEventSubscriberCollection(IServiceProvider serviceProvider) {
            this.serviceProvider = serviceProvider;
        }

        public IEnumerable<IDomainEventSubscriber<TEvent>> GetSubscibers<TEvent>() where TEvent : IDomainEvent {
            return serviceProvider.GetServices<IDomainEventSubscriber<TEvent>>();
        }
    }
}