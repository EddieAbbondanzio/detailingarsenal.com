
using System.Threading.Tasks;

public interface ISubscriptionService : IService {
    Task<Subscription> CreateTrialSubscription(Customer customer);
}