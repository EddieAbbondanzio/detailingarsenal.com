using System;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface ISubscriptionService : IService {
        Task<SubscriptionUpdate> StartTrialSubscription(Customer customer);
    }

}