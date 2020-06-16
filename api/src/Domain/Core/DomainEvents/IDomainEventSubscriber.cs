
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Core {
    /// <summary>
    /// Subscriber that wants to listen for a particular domain event.
    /// </summary>
    public interface IDomainEventSubscriber<TEvent> where TEvent : IDomainEvent {
        #region Publics
        Task Notify(TEvent busEvent);
        #endregion
    }
}