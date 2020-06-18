using System;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface ISubscriptionService : IService {
        Task<SubscriptionUpdate> StartTrialSubscription(Customer customer);
    }

    public class SubscriptionService : ISubscriptionService {
        // private ISubscriptionConfig config;
        // private ISubscriptionPlanRepo planRepo;
        // private ISubscriptionGateway subscriptionGateway;

        public async Task<SubscriptionUpdate> StartTrialSubscription(Customer customer) {
            // var trialPlan = await GetTrialPlan();
            // var planInfo = await subscriptionGateway.CreateTrialSubscription(trialPlan, customer);
            throw new System.Exception("FUCK");

        }

        // async Task<SubscriptionPlan> GetTrialPlan() {
        //     var trialPlan = await planRepo.FindByExternalId(config.DefaultPlan);

        //     if (trialPlan == null) {
        //         throw new InvalidOperationException("No trial plan specified in subscription config");
        //     }

        //     return trialPlan;
        // }
    }
}