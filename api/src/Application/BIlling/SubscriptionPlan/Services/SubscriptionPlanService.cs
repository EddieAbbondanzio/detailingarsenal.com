using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class SubscriptionService : IService {
        private ISubscriptionPlanInfoGateway infoGateway;
        private ISubscriptionPlanRepo repo;

        public SubscriptionService(ISubscriptionPlanInfoGateway infoGateway, ISubscriptionPlanRepo repo) {
            this.infoGateway = infoGateway;
            this.repo = repo;
        }

        public async Task<List<SubscriptionPlan>> RefreshPlans() {
            var planInfos = await infoGateway.GetAll();
            var plans = new List<SubscriptionPlan>();

            foreach (SubscriptionPlanInfo info in planInfos) {
                var plan = await repo.FindByExternalId(info.ExternalId);

                if (plan == null) {
                    plan = new SubscriptionPlan() {
                        Id = Guid.NewGuid(),
                        Name = info.Name,
                        ExternalId = info.ExternalId,
                        Prices = info.Prices.Select(p => new SubscriptionPlanPrice() {
                            Id = Guid.NewGuid(),
                            ExternalId = p.ExternalId,
                            Price = p.Price,
                            Interval = p.Interval
                        }).ToList()
                    };

                    await repo.Add(plan);
                } else {
                    plan.Prices = info.Prices.Select(p => new SubscriptionPlanPrice() {
                        Id = Guid.NewGuid(),
                        Price = p.Price,
                        ExternalId = info.ExternalId,
                        Interval = p.Interval
                    }).ToList();

                    await repo.Update(plan);
                }

                plans.Add(plan);
            }

            return plans;
        }
    }
}