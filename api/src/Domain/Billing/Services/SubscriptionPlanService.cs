using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Domain.Billing {
    public interface ISubscriptionPlanService : IService {
        Task<SubscriptionPlan> GetTrialPlan();
        Task<List<SubscriptionPlan>> RefreshPlans();
    }

    public class SubscriptionPlanService : ISubscriptionPlanService {
        ISubscriptionPlanGateway gateway;
        ISubscriptionConfig config;
        SubscriptionPlan? trialPlan;

        public SubscriptionPlanService(ISubscriptionConfig config, ISubscriptionPlanGateway gateway) {
            this.config = config;
            this.gateway = gateway;
        }

        public async Task<SubscriptionPlan> GetTrialPlan() {
            if (trialPlan == null) {
                trialPlan = await gateway.GetByExternalId(config.DefaultPlan);
            }

            return trialPlan;
        }

        public async Task<List<SubscriptionPlan>> RefreshPlans() {
            return await gateway.RefreshPlans();
        }
    }
}