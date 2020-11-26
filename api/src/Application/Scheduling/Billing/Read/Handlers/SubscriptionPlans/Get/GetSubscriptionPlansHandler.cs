using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Scheduling.Billing {
    [Authorization(Action = "read", Scope = "subscription-plans")]
    public class GetSubscriptionPlansHandler : ActionHandler<GetSubscriptionPlansQuery, List<SubscriptionPlanView>> {
        private ISubscriptionPlanService service;
        private IMapper mapper;

        public GetSubscriptionPlansHandler(ISubscriptionPlanService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<List<SubscriptionPlanView>> Execute(GetSubscriptionPlansQuery input, User? user) {
            var plans = await service.GetAllPlans();
            return mapper.Map<List<SubscriptionPlan>, List<SubscriptionPlanView>>(plans);
        }
    }
}