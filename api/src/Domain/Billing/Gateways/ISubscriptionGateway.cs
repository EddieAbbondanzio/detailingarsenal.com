
using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface ISubscriptionGateway : IService {
        Task<Subscription> CreateTrialSubscription(Customer customer);
    }
}