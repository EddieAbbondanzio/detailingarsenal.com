using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Billing {
    public interface ICustomerGateway : IGateway {
        Task<Customer> GetByBillingId(string billingId);
        Task<Customer> CreateTrialCustomer(User user, SubscriptionPlan trialPlan);
        Task CancelSubscriptionAtPeriodEnd(Customer customer);
        Task UndoCancellingSubscription(Customer customer);
        Task Delete(Customer customer);
    }
}