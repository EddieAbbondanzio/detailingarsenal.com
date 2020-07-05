using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Billing {
    public interface ISessionGateway : IGateway {
        Task<BillingReference> CreateSession(Customer customer, string priceBillingId);
    }
}