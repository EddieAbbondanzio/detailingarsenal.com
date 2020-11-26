using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Scheduling.Billing {
    public interface ICheckoutSessionGateway : IGateway {
        Task<BillingReference> CreateSession(Customer customer, string priceBillingId);
    }
}