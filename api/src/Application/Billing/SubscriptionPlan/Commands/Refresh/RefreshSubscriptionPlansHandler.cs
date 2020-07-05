using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Billing {
    [Authorization(Action = "refresh", Scope = "subscription-plans")]
    public class RefreshSubscriptionPlansHandler : ActionHandler<RefreshSubscriptionPlansCommand, List<SubscriptionPlanView>> {
        ISubscriptionPlanService service;
        private IMapper mapper;

        public RefreshSubscriptionPlansHandler(ISubscriptionPlanService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<List<SubscriptionPlanView>> Execute(RefreshSubscriptionPlansCommand input, User? user) {
            var plans = await service.RefreshPlans();
            return mapper.Map<List<SubscriptionPlan>, List<SubscriptionPlanView>>(plans);
        }
    }
}