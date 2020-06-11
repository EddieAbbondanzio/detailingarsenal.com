using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Authorization(Action = "refresh", Scope = "subscription-plans")]
    public class RefreshSubscriptionPlansHandler : ActionHandler<RefreshSubscriptionPlansCommand, List<SubscriptionPlanDto>> {
        private ISubscriptionPlanInfoService service;
        private ISubscriptionPlanRepo repo;
        private IMapper mapper;

        public RefreshSubscriptionPlansHandler(ISubscriptionPlanInfoService service, ISubscriptionPlanRepo repo, IMapper mapper) {
            this.service = service;
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<List<SubscriptionPlanDto>> Execute(RefreshSubscriptionPlansCommand input, User? user) {
            var planInfos = await service.GetAll();
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

            return mapper.Map<List<SubscriptionPlan>, List<SubscriptionPlanDto>>(plans);
        }
    }
}