using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class SubscriptionPlanService : IService {
        private IExternalSubscriptionPlanGateway infoGateway;
        private ISubscriptionPlanRepo repo;

        public SubscriptionPlanService(IExternalSubscriptionPlanGateway infoGateway, ISubscriptionPlanRepo repo) {
            this.infoGateway = infoGateway;
            this.repo = repo;
        }

        public async Task<List<SubscriptionPlan>> RefreshPlans() {
            var planInfos = await infoGateway.GetAll();
            var plans = new List<SubscriptionPlan>();

            foreach (ExternalSubscriptionPlan info in planInfos) {
                var plan = await repo.FindByExternalId(info.ExternalId);

                if (plan == null) {
                    plan = SubscriptionPlan.Create(
                        info.Name,
                        info.ExternalId,
                        info.Prices.Select(
                            p => SubscriptionPlanPrice.Create(
                                p.ExternalId,
                                p.Price,
                                p.Interval
                            )
                        ).ToList()
                    );

                    await repo.Add(plan);
                } else {
                    plan.Prices = info.Prices.Select(
                        p => SubscriptionPlanPrice.Create(
                            p.ExternalId,
                            p.Price,
                            p.Interval
                        )
                    ).ToList();

                    await repo.Update(plan);
                }

                plans.Add(plan);
            }

            return plans;
        }
    }
}