
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application {
    [Authorization(Scope = "subscription-plans", Action = "update")]
    public class UpdateSubscriptionPlanHandler : ActionHandler<UpdateSubscriptionPlanCommand, SubscriptionPlanDto> {
        private ISubscriptionPlanRepo repo;
        private IMapper mapper;

        public UpdateSubscriptionPlanHandler(ISubscriptionPlanRepo repo, IMapper mapper) {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<SubscriptionPlanDto> Execute(UpdateSubscriptionPlanCommand input, User? user) {
            var plan = await repo.FindById(input.Id);

            if (plan == null) {
                throw new EntityNotFoundException();
            }

            plan.RoleId = input.RoleId;

            await repo.Update(plan);
            return mapper.Map<SubscriptionPlan, SubscriptionPlanDto>(plan);
        }
    }
}