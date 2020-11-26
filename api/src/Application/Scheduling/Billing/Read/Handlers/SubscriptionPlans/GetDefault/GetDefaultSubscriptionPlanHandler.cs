using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Scheduling.Billing {
    public class GetDefaultSubscriptionPlanHandler : ActionHandler<GetDefaultSubscriptionPlanQuery, SubscriptionPlanReadModel> {
        ISubscriptionPlanService planService;
        IMapper mapper;

        public GetDefaultSubscriptionPlanHandler(ISubscriptionPlanService planService, IMapper mapper) {
            this.planService = planService;
            this.mapper = mapper;
        }

        public async override Task<SubscriptionPlanReadModel> Execute(GetDefaultSubscriptionPlanQuery input, User? user) {
            var plan = await planService.GetDefaultPlan();
            return mapper.Map<SubscriptionPlan, SubscriptionPlanReadModel>(plan);
        }
    }
}