using System.Collections.Generic;

namespace DetailingArsenal.Domain {
    public interface IDomainEventSubscriberCollection {
        IEnumerable<IDomainEventSubscriber<TEvent>> GetSubscibers<TEvent>() where TEvent : IDomainEvent;
    }
}