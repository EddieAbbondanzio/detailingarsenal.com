
using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface IExternalSubscriptionGateway : IService {
        Task<Subscription> CreateTrialSubscription(SubscriptionPlan plan, Customer customer);
    }
}