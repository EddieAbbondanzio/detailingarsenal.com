using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Authorization(Action = "refresh", Scope = "subscription-plans")]
    public class RefreshSubscriptionPlansHandler : ActionHandler<RefreshSubscriptionPlansCommand, List<SubscriptionPlanDto>> {
        SubscriptionService service;
        private IMapper mapper;

        public RefreshSubscriptionPlansHandler(SubscriptionService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<List<SubscriptionPlanDto>> Execute(RefreshSubscriptionPlansCommand input, User? user) {
            var plans = await service.RefreshPlans();
            return mapper.Map<List<SubscriptionPlan>, List<SubscriptionPlanDto>>(plans);
        }
    }
}