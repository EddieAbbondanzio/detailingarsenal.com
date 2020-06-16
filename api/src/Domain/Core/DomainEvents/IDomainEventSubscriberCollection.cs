using System.Collections.Generic;

namespace DetailingArsenal.Domain.Core {
    public interface ISubscriberCollection {
        IEnumerable<IDomainEventHandler<TEvent>> GetSubscibers<TEvent>() where TEvent : IDomainEvent;
    }
}