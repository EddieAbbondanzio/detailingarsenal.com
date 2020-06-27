
using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface ISubscriptionGateway : IGateway {
        Task<Subscription> CreateTrialSubscription(SubscriptionPlan plan, Customer customer);
    }
}