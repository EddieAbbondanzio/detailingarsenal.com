using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Billing {
    public class GetDefaultSubscriptionPlanHandler : ActionHandler<GetDefaultSubscriptionPlanQuery, SubscriptionPlanView> {
        ISubscriptionPlanService planService;
        IMapper mapper;

        public GetDefaultSubscriptionPlanHandler(ISubscriptionPlanService planService, IMapper mapper) {
            this.planService = planService;
            this.mapper = mapper;
        }

        public async override Task<SubscriptionPlanView> Execute(GetDefaultSubscriptionPlanQuery input, User? user) {
            var plan = await planService.GetDefaultPlan();
            return mapper.Map<SubscriptionPlan, SubscriptionPlanView>(plan);
        }
    }
}