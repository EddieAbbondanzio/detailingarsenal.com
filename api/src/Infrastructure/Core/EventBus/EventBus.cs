using DetailingArsenal.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Infrastructure {
    /// <summary>
    /// Event dispatcher to handle notifying subscribers when an event occurs.
    /// </summary>
    public class EventBus : IEventBus {
        private IServiceProvider serviceProvider;

        public EventBus(IServiceProvider serviceProvider) {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Dispatch an event to any handler that wants to know about it.
        /// </summary>
        /// <param name="busEvent">The domain event to dispatch.</param>
        /// <typeparam name="TEvent">The type of event.</typeparam>
        public async Task Dispatch<TEvent>(TEvent busEvent) where TEvent : class, IBusEvent {
            Type eventType = typeof(TEvent);
            IEnumerable<IBusEventHandler<TEvent>> handlers = serviceProvider.GetServices<IBusEventHandler<TEvent>>();

            foreach (IBusEventHandler<TEvent> handler in handlers) {
                await handler.Handle(busEvent);
            }
        }
    }
}