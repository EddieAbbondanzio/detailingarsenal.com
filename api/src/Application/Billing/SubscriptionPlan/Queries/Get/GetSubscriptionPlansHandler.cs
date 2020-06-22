using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Authorization(Action = "read", Scope = "subscription-plans")]
    public class GetSubscriptionPlansHandler : ActionHandler<GetSubscriptionPlansQuery, List<SubscriptionPlanDto>> {
        private ISubscriptionPlanRepo repo;
        private IMapper mapper;

        public GetSubscriptionPlansHandler(ISubscriptionPlanRepo repo, IMapper mapper) {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<List<SubscriptionPlanDto>> Execute(GetSubscriptionPlansQuery input, User? user) {
            var plans = await repo.FindAll();
            return mapper.Map<List<SubscriptionPlan>, List<SubscriptionPlanDto>>(plans);
        }
    }
}