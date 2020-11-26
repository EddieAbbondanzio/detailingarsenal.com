using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Domain.Scheduling.Billing {
    public interface ISubscriptionPlanRefresher : IService {
        Task<List<SubscriptionPlan>> RefreshPlans();
    }

    public class SubscriptionPlanRefresher : ISubscriptionPlanRefresher {
        ISubscriptionPlanGateway gateway;
        ISubscriptionPlanRepo repo;
        IBillingConfig config;

        public SubscriptionPlanRefresher(ISubscriptionPlanGateway gateway, ISubscriptionPlanRepo repo, IBillingConfig config) {
            this.gateway = gateway;
            this.repo = repo;
            this.config = config;
        }


        // Don't delete. It's used twice in the app layer.
        public async Task<List<SubscriptionPlan>> RefreshPlans() {
            var plans = await gateway.GetAll();

            foreach (var plan in plans) {
                var existingPlan = await repo.FindById(plan.Id);

                if (existingPlan == null) {
                    await repo.Add(plan);
                } else {
                    // We don't want to wipe out some extra data we keep on our end. (Description, RoleId)
                    existingPlan.Name = plan.Name;
                    existingPlan.Prices = plan.Prices;

                    await repo.Update(existingPlan);
                }
            }

            return plans;
        }
    }
}