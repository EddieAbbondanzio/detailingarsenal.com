using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using System.Linq;

namespace DetailingArsenal.Domain.Billing {
    /// <summary>
    /// Service that supports parsing billing webhook events from their raw streams and
    /// converting them into the corresponding domain event.
    /// </summary>
    public interface IBillingWebhookParser : IService {
        Task<TDomainEvent?> Parse<TDomainEvent>(Stream stream, string signature) where TDomainEvent : class, IDomainEvent;
    }
}