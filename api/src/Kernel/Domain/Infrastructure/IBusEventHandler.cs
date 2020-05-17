
using System.Threading.Tasks;

/// <summary>
/// Handler to process a bus event.
/// </summary>
/// <typeparam name="TEvent">Event type it can handle.</typeparam>
public interface IBusEventHandler<TEvent> where TEvent : IBusEvent {
    #region Publics
    /// <summary>
    /// Handle a doamin event.
    /// </summary>
    /// <param name="busEvent">The domain event to handle.</param>
    Task Handle(TEvent busEvent);
    #endregion
}