using System.Collections.Generic;

namespace DetailingArsenal.Domain.Core {
    public interface IDomainEventSubscriberCollection {
        IEnumerable<IDomainEventSubscriber<TEvent>> GetSubscibers<TEvent>() where TEvent : IDomainEvent;
    }
}