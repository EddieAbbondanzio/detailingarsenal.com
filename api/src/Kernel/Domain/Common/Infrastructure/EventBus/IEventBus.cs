using System;
using System.Threading.Tasks;

/// <summary>
/// Bus for propogating events to help keep modules loosely coupled with each other.
/// </summary>
public interface IEventBus {
    #region Publics
    /// <summary>
    /// Dispatch the event to any handlers that may be waiting for it.
    /// </summary>
    /// <param name="busEvent">The event to raise.</param>
    /// <typeparam name="TEvent">Type of event.</typeparam>
    Task Dispatch<TEvent>(TEvent busEvent) where TEvent : class, IBusEvent;
    #endregion
}