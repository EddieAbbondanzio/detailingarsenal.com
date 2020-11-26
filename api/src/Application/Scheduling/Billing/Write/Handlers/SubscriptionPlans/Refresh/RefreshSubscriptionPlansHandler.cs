using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Scheduling.Billing {
    [Authorization(Action = "refresh", Scope = "subscription-plans")]
    public class RefreshSubscriptionPlansHandler : ActionHandler<RefreshSubscriptionPlansCommand, List<SubscriptionPlanReadModel>> {
        ISubscriptionPlanService service;
        private IMapper mapper;

        public RefreshSubscriptionPlansHandler(ISubscriptionPlanService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<List<SubscriptionPlanReadModel>> Execute(RefreshSubscriptionPlansCommand input, User? user) {
            var plans = await service.RefreshPlans();
            return mapper.Map<List<SubscriptionPlan>, List<SubscriptionPlanReadModel>>(plans);
        }
    }
}