using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Billing {
    public interface ICustomerGateway : IGateway {
        Task<Customer> CreateTrialCustomer(User user, SubscriptionPlan trialPlan);
        Task Delete(Customer customer);
    }
}