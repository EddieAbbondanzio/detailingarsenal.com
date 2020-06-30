using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Billing {
    [Authorization(Action = "read", Scope = "subscription-plans")]
    public class GetSubscriptionPlansHandler : ActionHandler<GetSubscriptionPlansQuery, List<SubscriptionPlanDto>> {
        private ISubscriptionPlanService service;
        private IMapper mapper;

        public GetSubscriptionPlansHandler(ISubscriptionPlanService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<List<SubscriptionPlanDto>> Execute(GetSubscriptionPlansQuery input, User? user) {
            var plans = await service.GetAllPlans();
            return mapper.Map<List<SubscriptionPlan>, List<SubscriptionPlanDto>>(plans);
        }
    }
}