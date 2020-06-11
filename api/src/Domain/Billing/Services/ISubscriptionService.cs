
using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface ISubscriptionService : IService {
        Task<Subscription> CreateTrialSubscription(Customer customer);
    }
}