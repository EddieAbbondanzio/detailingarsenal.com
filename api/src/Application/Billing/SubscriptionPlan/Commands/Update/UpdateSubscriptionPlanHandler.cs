
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Billing {
    [Authorization(Scope = "subscription-plans", Action = "update")]
    public class UpdateSubscriptionPlanHandler : ActionHandler<UpdateSubscriptionPlanCommand, SubscriptionPlanDto> {
        ISubscriptionPlanService service;
        IMapper mapper;

        public UpdateSubscriptionPlanHandler(ISubscriptionPlanService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<SubscriptionPlanDto> Execute(UpdateSubscriptionPlanCommand input, User? user) {
            var plan = await service.GetById(input.Id);

            await service.Update(plan, new UpdateSubscriptionPlan(input.RoleId));
            return mapper.Map<SubscriptionPlan, SubscriptionPlanDto>(plan);
        }
    }
}