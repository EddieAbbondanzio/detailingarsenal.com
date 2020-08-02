using System.IO;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Domain.Billing {
    public interface IBillingEventParser {
        /// <summary>
        /// Check to see if the event can be parsed by this parser. 
        /// </summary>
        /// <param name="eventType">The string identifier of the event.</param>
        /// <returns>True if the event can be parsed.</returns>
        bool CanParse(string eventType);

        Task<IDomainEvent> Parse(Stream raw);
    }
}